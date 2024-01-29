using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Posemesh.Pocketbase.Model;
using UnityEngine;
using UnityEngine.Networking;

namespace Posemesh.Pocketbase
{
    /// <summary>
    /// CRUD operations for PocketBase christmas_trees collection
    /// </summary>
    public class PocketbaseApiClient : MonoBehaviour
    {
        private string _apiUrl = "http://127.0.0.1:8090/api";
        
        public void GetTreesInDomain(string domainId, Action<List<ChristmasTreeData>> onComplete)
        {
            string filter = $"(domainId='{domainId}')";
            filter = HttpUtility.UrlEncode(filter);
            string url = $"{_apiUrl}/collections/christmas_trees/records?filter={filter}";
            UnityWebRequest request = new UnityWebRequest(url);
            request.method = "GET";
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SendWebRequest().completed += operation =>
            {
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log("Response: " + request.downloadHandler.text);
                    GetTreesResponse response = JsonUtility.FromJson<GetTreesResponse>(request.downloadHandler.text);
                    onComplete?.Invoke(response.items);
                    Debug.Log(response.items.Count);
                }

                request.Dispose();
            };
        }

        public void AddTreeToDomain(string domainId, Pose pose, TreeData treeData, Action<ChristmasTreeData> onComplete)
        {
            string url = $"{_apiUrl}/collections/christmas_trees/records/";
            UnityWebRequest request = new UnityWebRequest(url);
            request.method = "POST";
            var json = JsonUtility.ToJson(new ChristmasTreeData
            {
                domainId = domainId,
                pose = SerializablePose.FromPose(pose),
                data = treeData
            });
            Debug.Log(json);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest().completed += operation =>
            {
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log("Create Response: " + request.downloadHandler.text);
                    ChristmasTreeData christmasTreeData = JsonUtility.FromJson<ChristmasTreeData>(request.downloadHandler.text);
                    onComplete?.Invoke(christmasTreeData);
                }
            };
        }

        public void UpdateTree(string treeId, string domainId, Pose pose, TreeData treeData, Action<ChristmasTreeData> onComplete)
        {
            string url = $"{_apiUrl}/collections/christmas_trees/records/{treeId}";
            Debug.Log(url);
            
            var json = JsonUtility.ToJson(new ChristmasTreeData
            {
                id = treeId,
                domainId = domainId,
                pose = SerializablePose.FromPose(pose),
                data = treeData
            });
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            UnityWebRequest request = UnityWebRequest.Put(url, bodyRaw);
            request.method = "PATCH";
            Debug.Log(json);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Content-length", (bodyRaw.Length.ToString()));

            request.SendWebRequest().completed += operation =>
            {
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log("Update Response: " + request.downloadHandler.text);
                    ChristmasTreeData christmasTreeData = JsonUtility.FromJson<ChristmasTreeData>(request.downloadHandler.text);
                    onComplete?.Invoke(christmasTreeData);
                }
            };
        }
        
        public void DeleteTree(string treeId, Action onComplete)
        {
            string url = $"{_apiUrl}/collections/christmas_trees/records/{treeId}";
            UnityWebRequest request = new UnityWebRequest(url);
            request.method = "DELETE";
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest().completed += operation =>
            {
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log("Delete Response: " + request.downloadHandler.text);
                    onComplete?.Invoke();
                }
            };
        }
    }
}