using UnityEngine;
using Project.UI.ProgressBar;
using System;
using Firebase.Storage;
using System.Collections.Generic;
using Project.AssetIO.Firebase;
using System.Threading.Tasks;
using System.Threading;

namespace Project.UI.Panel
{
    public class DownloadingPanelView : LoadingPanelView
    {

        public struct DownloadTask
        {
            public StorageReference reference;
            public int totalSize;
            public int currentVal;
            public string destinationFile;
            public Action<int, DownloadTask> onFinish;
        }
        List<DownloadTask> downloadTasks;
        DownloadTask currentTask;
        [SerializeField] protected SlideBarController slideBarController;
        public async void AddDownloadTask(StorageReference reference, string fullFilePath, Action<int, DownloadTask> onFinish)
        {

            downloadTasks ??= new(10);
            
            int fileSize = (int)(await reference.GetMetadataAsync()).SizeBytes / 1024;
            DownloadTask task = new()
            {
                reference = reference,
                totalSize = fileSize,
                currentVal = 0,
                destinationFile = fullFilePath,
                onFinish = onFinish,
            };
            downloadTasks.Add(task);
        }

        private void UpdateProgress(DownloadState state)
        {

            currentTask.currentVal = (int)(state.BytesTransferred / 1024);
            this.slideBarController.UpdateEndValue(currentTask.currentVal);
            this.slideBarController.StartAnimation();
        }
        public async Task StartDownload()
        {
            if(downloadTasks == null || downloadTasks.Count == 0) {
                slideBarController.SetupAnimation(1);
                slideBarController.UpdateEndValue(1);
                slideBarController.StartAnimation();
                return;
            }

            await ShowAsync();
            for(int i = 0; i < downloadTasks.Count; i++)
            {
                currentTask = downloadTasks[i];
                slideBarController.SetupAnimation(currentTask.totalSize);
                await FirebaseStorageDownloadHandler.DownloadFile(currentTask.reference, currentTask.destinationFile, UpdateProgress);
                currentTask.onFinish?.Invoke(i, downloadTasks[i]);
            }
            await HideAsync();
        }
    }
}