using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiniApp.Model
{
    public class FireModellib
    {
        private IFirebaseClient cliente;

        public FireModellib()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "bTTsVUtjiyJBtyRhCf8KlfdZHifExxuBbmJ7vDTk",
                BasePath = "https://kinieapp-fire.firebaseio.com/"
            };
            cliente = new FirebaseClient(config);
        }

        public async Task<T> SetFireDataAsync<T>(T info, string path)
        {
            //SetResponse response = await cliente.SetAsync("todos/set", todo);
            SetResponse response = await cliente.SetAsync(path, info);
            T result = response.ResultAs<T>(); //The response will contain the data written
            return result;
        }
        public async Task<string> PushFireDataAsync<T>(T info, string path)
        {
          
            PushResponse response = await cliente.PushAsync(path, info);
            return response.Result.name; //The result will contain the child name of the new data that was added
        }

        public async Task<T> GetFireDataAsync<T>(T info, string path)
        {
            FirebaseResponse response = await cliente.GetAsync(path);
            T todo = response.ResultAs<T>(); //The response will contain the data being retreived
            return todo;
        }
        public async Task<T> UpdateFireDataAsync<T>(T info, string path)
        {
            FirebaseResponse response = await cliente.UpdateAsync(path, info);
            T todo = response.ResultAs<T>(); //The response will contain the data written
            return todo;
        }

        public async Task<System.Net.HttpStatusCode> UpdateFireDataAsync<T>( string path)
        {
            FirebaseResponse response = await cliente.DeleteAsync(path); //Deletes todos collection
            var temp = response.StatusCode;
            return temp;
        }
    }
}
