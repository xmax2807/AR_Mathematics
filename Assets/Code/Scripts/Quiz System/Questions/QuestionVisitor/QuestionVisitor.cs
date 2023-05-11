using UnityEngine;
using Project.UI.UISetPack;
using Project.AssetIO.Firebase;
using System.Threading.Tasks;
using Project.AssetIO;

namespace Project.QuizSystem
{
    public class QuestionVisitor : IQuestionVisitor
    {
        private GroupUISpritePack groupPack;
        private FileDataCacheHandler<Texture2D> FileHandler;
        public QuestionVisitor(GroupUISpritePack pack)
        {
            groupPack = pack;
            FileHandler = new FirestorageFileDataCacheHandler<Texture2D>(new TextureFileHandler());
        }

        public Task<Sprite> AskForSprite(ShapeSCQ imageQuestion)
        {
            Shape.ShapeType shapeType = imageQuestion.GetImageType();
            ShapeUISpritePack shapePack = groupPack.FindCompositePack("ShapePacks") as ShapeUISpritePack;
            if (shapePack == null) return null;
            return Task.FromResult<Sprite>(shapePack.PickASpriteRandomly(shapeType));
        }

        public async Task<Sprite> AskForSprite(GeneralQuestion generalQuestion)
        {
            string url = generalQuestion.GetImageType();
            Texture2D result = await FileHandler.GetFile(url);
            return Sprite.Create(result, new Rect(0.0f, 0.0f, result.width, result.height), new Vector2(0.5f, 0.5f), 100f);
        }
    }
}