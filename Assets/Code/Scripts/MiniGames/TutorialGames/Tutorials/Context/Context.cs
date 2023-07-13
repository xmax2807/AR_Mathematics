namespace Project.MiniGames.TutorialGames{
    public class Context
    {
        private ITutorialCommand[] commands;
        private int currentIndex;

        public Context(ITutorialCommand[] commands){
            currentIndex = 0;
            this.commands = new ITutorialCommand[commands.Length];
            for(int i = 0; i < commands.Length; ++i){
                this.commands[i] = commands[i];
            }
        }
        public Context(CommandSO[] commands){
            currentIndex = 0;
            this.commands = new ITutorialCommand[commands.Length];
            for(int i = 0; i < commands.Length; ++i){
                this.commands[i] = commands[i].BuildCommand();
            }
        }

        public ITutorialCommand CurrentCommand => commands[currentIndex];
        public ITutorialCommand NextCommand{
            get{
                ++currentIndex;
                return commands[currentIndex];
            }
        }

        public ITutorialCommand PrevCommand{
            get{
                --currentIndex;
                return commands[currentIndex];
            }
        }
    }
}