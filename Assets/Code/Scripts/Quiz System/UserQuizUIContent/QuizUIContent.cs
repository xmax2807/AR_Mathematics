using Project.Managers;
using UnityEngine;
using UnityEngine.UI;
using Project.QuizSystem;
using System.Threading.Tasks;

namespace Project.QuizSystem.QuizUIContent{
    public abstract class QuizUIContent{
        protected abstract QuestionContentType contentType {get;}
        protected GameObject prefab;
        protected IQuestion question;
        protected IQuestionVisitor Visitor;
        public QuizUIContent(GameObject prefab, IQuestionVisitor visitor){
            this.prefab = prefab;
            this.Visitor = visitor;
        }
        public async Task CreateUI(Transform parent, IQuestion question){
            if(question.QuestionContentType != contentType){
                throw new UnassignedReferenceException("Question's content type is not match with this UI");
            }

            await GetQuestionInfo(question);
            BuildUI(parent);
        }
        protected abstract void BuildUI(Transform parent);
        public virtual Task GetQuestionInfo(IQuestion question){
            this.question = question;
            return Task.CompletedTask;
        }
    }
    public abstract class QuizUIContent<T> : QuizUIContent where T : Component
    {
        protected T mainComponent;
        protected QuizUIContent(GameObject prefab, IQuestionVisitor visitor) : base(prefab, visitor)
        {
            if(!prefab.TryGetComponent<T>(out mainComponent)){
                throw new MissingComponentException($"Prefab missing {mainComponent.GetType().Name}");
            }
        }
        protected override void BuildUI(Transform parent)
        {
            SpawnerManager.Instance.SpawnComponentInParent(mainComponent,parent,OnBuildUI);
        }
        protected abstract void OnBuildUI(T component);
    }
    public class TMProUIQuizContent : QuizUIContent<TMPro.TextMeshProUGUI>
    {
        protected override QuestionContentType contentType => QuestionContentType.Text;
        private string questContent;

        public TMProUIQuizContent(GameObject prefab, IQuestionVisitor visitor) : base(prefab, visitor){}

        protected override void OnBuildUI(TMPro.TextMeshProUGUI textMesh){
            textMesh.text = questContent;
        }
        public override async Task GetQuestionInfo(IQuestion question)
        {
            await base.GetQuestionInfo(question);
            questContent = question.GetQuestion();
        }
    }
    public class ImageQuizContent : QuizUIContent<Image>
    {
        protected override QuestionContentType contentType => QuestionContentType.Image;
        private Sprite sprite;
        public ImageQuizContent(GameObject prefab, IQuestionVisitor visitor) : base(prefab, visitor){}

        protected override void OnBuildUI(Image image){
            image.sprite = sprite;
            image.preserveAspect = true;
        }
        public override async Task GetQuestionInfo(IQuestion question)
        {
            await base.GetQuestionInfo(question);
            // if(sprite != null){
            //     return;
            // }
            if(question is IVisitableImageQuestion imageQuestion) {
                sprite = await imageQuestion.AcceptVisitor(this.Visitor);
            }
        }
    }
}