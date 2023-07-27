using UnityEngine;
using Project.UI.UISetPack;
using Project.AssetIO.Firebase;
using System.Threading.Tasks;
using Project.AssetIO;
using System.Collections.Generic;

namespace Project.QuizSystem
{
    public class QuestionVisitor : IQuestionVisitor
    {
        private GroupUISpritePack groupPack;
        private FileDataCacheHandler<Texture2D> FileHandler;
        private Dictionary<string, Task> workingProgresses;
        public QuestionVisitor(GroupUISpritePack pack)
        {
            groupPack = pack;
            FileHandler = new FirestorageFileDataCacheHandler<Texture2D>(new TextureFileHandler());
            workingProgresses = new Dictionary<string, Task>();
        }

        public Task<Sprite> AskForSprite(ShapeSCQ imageQuestion)
        {
            if(imageQuestion.spriteName != ""){
                return groupPack.FindASprite(imageQuestion.spriteName);
            }
            Shape.ShapeType shapeType = imageQuestion.GetImageType();
            ShapeUISpritePack shapePack = groupPack.FindCompositePack("ShapePacks") as ShapeUISpritePack;
            if (shapePack == null) return null;
            return Task.FromResult<Sprite>(shapePack.PickASpriteRandomly(shapeType));
        }

        public async Task<Sprite> AskForSprite(GeneralQuestion generalQuestion)
        {
            string url = generalQuestion.GetImageType();
            if(workingProgresses.ContainsKey(url)){
                await workingProgresses[url];
            }

            Task<Texture2D> task = FileHandler.GetFile(url);
            workingProgresses[url] = task;

            Texture2D result = await task;
            if(result == null) return null;

            return Sprite.Create(result, new Rect(0.0f, 0.0f, result.width, result.height), new Vector2(0.5f, 0.5f), 100f);
        }
    }
}