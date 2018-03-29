using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace GoalList.Services
{
    class PlayMediaFilesFromPackageFolderService
    {

        /// <summary>
        /// This is the method that plays the media tracks (on done and add) 
        /// </summary>
        /// <remarks>
        ///     It gets (opens) the given folder, and gets (opens) the given file and I create a
        ///     <c>MediaElement</c> object to confirm that the file is media, and then play it!!
        /// </remarks>
        /// <param name="folderName">Contains the folder's name that contains the media file</param>
        /// <param name="fileName">Contains the media file's name in the <c>folderName</c></param>
        internal static async void PlayMedia(string folderName, string fileName)
        {
            StorageFolder folder = Package.Current.InstalledLocation;
            folder = await Package.Current.InstalledLocation.GetFolderAsync(folderName);
            StorageFile file = await folder.GetFileAsync(fileName);
            var music = new MediaElement();
            music.SetSource(await file.OpenReadAsync(), file.ContentType);
            music.Play();
        }
    }
}
