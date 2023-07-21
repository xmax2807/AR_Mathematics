using System;
using System.Collections;
using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public class Stage : IStage
    {
        private Context _context;
        private ICommander _commander;
        private ITutorialCommand _currentCommand;

        private Coroutine _coroutine;

        public int CurrentCommandIndex => _context.CurrentIndex;

        public Stage(Context ctx, ICommander commander){
            this._context = ctx;
            this._commander = commander;
        }

        public void Begin(){
            UnityEngine.Debug.Log("Begin Stage");
            _currentCommand = _context.CurrentCommand;
            _coroutine = _commander.GetExecuter().StartCoroutine(ExecuteCommand());
        }
        private IEnumerator ExecuteCommand(){
            yield return _currentCommand.Execute(_commander);
            Update();
        }
        public void End(){
            _commander.StageEnded();
        }
        public void Update(){
            Debug.Log("Update Stage");
            if(_context.NextCommand == null){
                End();
                return;
            }
            _currentCommand = _context.CurrentCommand;
            _coroutine = _commander.GetExecuter().StartCoroutine(ExecuteCommand());
        }

        public void MoveToCommand(int index)
        {
            Debug.Log("MoveToCommand");
            _commander.GetExecuter().StopCoroutine(_coroutine);
            _context.ToCommand(index);
            Begin();
        }
    }
}