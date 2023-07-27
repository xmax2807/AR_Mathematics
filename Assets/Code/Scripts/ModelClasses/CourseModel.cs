using System.Collections.Generic;
using Firebase.Firestore;
[FirestoreData]
public class CourseModel
{
    [FirestoreProperty] public List<int> chapCount { get; set; } 
}