namespace Project.MiniGames.TutorialGames{
    public class Context
    {
        private ITutorialCommand[] commands;
        private int currentIndex;
        public int CurrentIndex => currentIndex;

        public Context(ITutorialCommand[] commands){
            currentIndex = 0;
            this.commands = new ITutorialCommand[commands.Length];
            for(int i = 0; i < commands.Length; ++i){
                this.commands[i] = commands[i];
            }
        }
        public Context(Context clone){
            currentIndex = 0;
            this.commands = new ITutorialCommand[clone.commands.Length];
            for(int i = 0; i < this.commands.Length; ++i){
                this.commands[i] = clone.commands[i];
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
                if(currentIndex >= commands.Length - 1){
                    return null;
                }
                ++currentIndex;
                return commands[currentIndex];
            }
        }

        public ITutorialCommand PrevCommand{
            get{
                if(currentIndex <= 0){
                    return null;
                }
                --currentIndex;
                return commands[currentIndex];
            }
        }

        public ITutorialCommand ToCommand(int index){
            if(index < 0 || index >= commands.Length){
                return null;
            }
            currentIndex = index;
            return commands[index];
        }
    }
}