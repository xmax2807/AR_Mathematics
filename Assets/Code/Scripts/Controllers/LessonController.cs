
using UnityEngine;
using Firebase.Firestore;
using Project.Managers;
using System.Threading.Tasks;

[System.Serializable]
public class LessonData
{
    public string LessonTitle;
    public string LessonVideoFolder;
    public int VideoNumbers;
    public int LessonChapter;
    public int LessonSemester;
    public int LessonUnit;

    public bool LessonStatus;

}
public class LessonController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    Firebase.Storage.FirebaseStorage storage => DatabaseManager.Storage;
    public void UploadData<T>(string collection, System.Func<T> builder)
    {
        T model = builder.Invoke();

        db.Collection(collection).Document().SetAsync(model);
    }
    // public async Task<string> GetVideoUrl(int unit, int chapter)
    // {
    //     Query documentQuery = db.Collection("lessons").WhereEqualTo("LessonUnit", unit).WhereEqualTo("LessonChapter", chapter);
    //     QuerySnapshot snapshots = await documentQuery.GetSnapshotAsync();

    //     var model = snapshots[0].ConvertTo<LessonModel>();
    //     Debug.Log(model.GetFileFormat(7));
    //     Debug.Log(storage.GetReferenceFromUrl(model.GetFileFormat(7)).Path);
    // }
    public LessonModel LessonModel { get; private set; }
    public async Task<LessonModel> GetLessonModel(int unit, int chapter)
    {
        Query doc = db.Collection("lessons").WhereEqualTo("LessonUnit", unit).WhereEqualTo("LessonChapter", chapter);
        QuerySnapshot snapshot = await doc.GetSnapshotAsync();

        if (snapshot.Count == 0) return null;

        LessonModel = snapshot[0].ConvertTo<LessonModel>();
        return LessonModel;
    }

    public async Task<LessonModel> GetLessonModel(string lessonName)
    {
        Query doc = db.Collection("lessons").WhereEqualTo("LessonTitle", lessonName);
        QuerySnapshot snapshot = await doc.GetSnapshotAsync();

        if (snapshot.Count == 0) return null;

        LessonModel = snapshot[0].ConvertTo<LessonModel>();
        return LessonModel;
    }

    public async Task<System.Uri> GetVideoUrl(LessonModel model, int videoIndex)
    {
        try
        {
            string gsPath = model.GetFileFormat(videoIndex);
            var gsReference = storage.GetReferenceFromUrl(gsPath);

            var uri = await gsReference.GetDownloadUrlAsync();
            return uri;
        }
        catch (Firebase.FirebaseException e)
        {
            Debug.Log(e.Message);
            return null;
        }

    }

    public async Task<string> GetLessonID(int chapter, int unit)
    {
        string lessonID = null;
        Query queryLesson = db.Collection("lessons").WhereEqualTo("LessonUnit", unit).WhereEqualTo("LessonChapter", chapter);
        var result = await queryLesson.GetSnapshotAsync();
        foreach (DocumentSnapshot querySnapshot in result.Documents)
        {
            lessonID = querySnapshot.Id;
        }
        return lessonID;
    }
}