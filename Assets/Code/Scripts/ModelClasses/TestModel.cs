using Firebase.Firestore;
[FirestoreData]
public class TestModel
{
    // [FirestoreProperty] public string GameID { get; set; }
    [FirestoreProperty] public string TestTitle { get; set; }
    [FirestoreProperty] public int TestSemester { get; set; }
    [FirestoreProperty] public string[] TestQuestion { get; set; }
    [FirestoreProperty] public int TestTime { get; set; } //minute

}