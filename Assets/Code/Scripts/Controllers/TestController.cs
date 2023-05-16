using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TestController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;

    public async Task<(TestModel, QuizModel[])> GetTest(int semester)
    {
        List<TestModel> TestModels = new();
        Query queryTests = db.Collection("tests").WhereEqualTo("TestSemester", semester);
        var testSnapshot = await queryTests.GetSnapshotAsync();
        foreach (DocumentSnapshot test in testSnapshot.Documents)
        {
            var testModel = test.ConvertTo<TestModel>();
            TestModels.Add(testModel);
        }

        int index = Random.Range(0, TestModels.Count);
        List<QuizModel> quizModels = await DatabaseManager.Instance.QuizController.GetQuizzesByIDs(TestModels[index].TestQuestion);
        return (TestModels[index], quizModels.ToArray());

    }
}