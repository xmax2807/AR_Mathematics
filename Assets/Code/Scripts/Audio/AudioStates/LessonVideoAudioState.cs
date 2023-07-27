namespace Project.Audio{
    public sealed class LessonAudioState : SceneAudioState
    {
        public LessonAudioState(IAudioState wrappee) : base(wrappee)
        {
        }

        protected override void SelfEnterState()
        {
            base.SelfEnterState();
            Managers.AudioManager.Instance.BackgroundFX.Stop();
        }

        protected override void SelfExitState(){
            base.SelfExitState();
            Managers.AudioManager.Instance.BackgroundFX.Play();
        }
    }
}