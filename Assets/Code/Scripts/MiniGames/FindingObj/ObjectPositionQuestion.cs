using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Project.QuizSystem;
using System;
using Project.Managers;
using Project.Utils.ExtensionMethods;

public enum ObjectPosition
{
    Top = 1,
    Bottom = 2,
    Left = 3,
    Right = 4,
    Forward = 5,
    Backward = 6,
}
public enum ObjectFindingQuestionType
{
    PositionQuestionType,
    NameQuestionType
}
public struct ObjectFindingStruct : IEquatable<ObjectFindingStruct>
{
    public string Name;
    public ObjectPosition Position;

    public bool Equals(ObjectFindingStruct obj)
    {
        return Name == obj.Name || obj.Position== this.Position;
    }
}
public class ObjectPositionQuestion : BaseQuestion<int>, IRandomizableQuestion
{
    private static Dictionary<ObjectPosition, string> PosToString = new Dictionary<ObjectPosition, string>()
    {
        {ObjectPosition.Top,"trên"},
        {ObjectPosition.Bottom,"dưới"},
        {ObjectPosition.Left,"trái"},
        {ObjectPosition.Right,"phải"},
        {ObjectPosition.Forward,"trước"},
        { ObjectPosition.Backward,"sau"},
    };
    public string posit;

    
    public int numForPos;

    private ObjectFindingQuestionType questionType;
    
    public ObjectPositionQuestion(string question, ObjectFindingQuestionType questionType) : base(question)
    {
        this.questionType = questionType;
    }
    public ObjectPositionQuestion(string question) : this(question, ObjectFindingQuestionType.PositionQuestionType)
    {
    }


    public override QuestionType QuestionType => QuestionType.Other;

    public override QuestionContentType QuestionContentType => QuestionContentType.None;

    public override IQuestion Clone()
    {
        throw new System.NotImplementedException();
    }

    public IQuestion GetClone()
    {
        return new ObjectPositionQuestion(this._question, questionType);
    }
    public override string GetQuestion()
    {
        return "Chọn vật có vị trí " + PosToString[(ObjectPosition)_answer] + " so với khối lập phương trắng ?";
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        posit = posits[Random.Range(0, posits.Count)];
    //        aimText.text = "Chọn vật có vị trí " + posit + " so với khối lập phương trắng ?";
    //        Debug.Log("Random question");
    //    }
    //}

    //public void PositionToNumber()
    //{
    //    if (posit == "trên")
    //    {
    //        numForPos = 1;
    //    }
    //    else if (posit == "dưới")
    //    {
    //        numForPos = 2;
    //    }
    //    else if (posit == "trái")
    //    {
    //        numForPos = 3;
    //    }
    //    else if (posit == "phải")
    //    {
    //        numForPos = 4;
    //    }
    //    else if (posit == "trước")
    //    {
    //        numForPos = 5;
    //    }
    //    else if (posit == "sau")
    //    {
    //        numForPos = 6;
    //    }
    //}

    public IQuestion Random(System.Random rand = null)
    {
        var instance = new ObjectPositionQuestion(this._question, questionType);
        instance.Randomize(rand);
        return instance;
    }

    public void Randomize(System.Random rand = null)
    {
        rand ??= SpawnerManager.RandomInstance;
        questionType = FlagExtensionMethods.Randomize<ObjectFindingQuestionType>(rand);
        _answer = rand.Next(0, 6) + 1;
    }
}
