using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class TinyHouseCarController : MonoBehaviour, IAreaElement
    {
        [SerializeField] private TinyHouseCar tinyHouseCarPrefab;

        [Title("Target Points")]
        [SerializeField] private Transform targetPointAfterLoadSuccesfully;
        [SerializeField] private Transform carStartPoint;
        [SerializeField] private Transform sellAreaArrivePoint;

        [Title("Times")] 
        [SerializeField] private float spawnInterval;
        
        [Title("Tutorial")] 
        [SerializeField] private TutorialData firstJobCompletedTutorial;
        
        
        public TinyHouseCar CurrentCar => _currentCar;

        private WaitForSeconds _spawnIntervalWfs;
        private TinyHouseCar _currentCar;
        
        public Action<int, int> OnTinyHouseCompleted;

        public int TinyHouseSpawnCount
        {
            get => ES3.Load("TinyHouseSpawnCount_Save" + gameObject.name, 1);
            set => ES3.Save("TinyHouseSpawnCount_Save" + gameObject.name, value);
        }

        public void InitializeTinyHouseController()
        {
            SpawnTinyHouseCar();
        }

        private void SpawnTinyHouseCar()
        {
            _currentCar = Instantiate(tinyHouseCarPrefab, carStartPoint.position, tinyHouseCarPrefab.transform.rotation);
            CurrentCar.InitCar(targetPointAfterLoadSuccesfully.position, sellAreaArrivePoint.position, this, TinyHouseSpawnCount++);
            CurrentCar.transform.SetParent(transform);
            OnTinyHouseCarSpawned?.Invoke(CurrentCar);
            CurrentCar.OnArrivedExitArea += WaitForNextSpawnInterval;
        }

        private void WaitForNextSpawnInterval()
        {
            _spawnIntervalWfs = new WaitForSeconds(.1f); 
            CurrentCar.OnArrivedExitArea -= WaitForNextSpawnInterval;

            StartCoroutine(IE_WaitForSpawnInterval());

            IEnumerator IE_WaitForSpawnInterval()
            {
                yield return _spawnIntervalWfs;
                
                // Destroy existing car
                Destroy(CurrentCar.gameObject);
                _currentCar = null;
                
                // Spawn new one.
                SpawnTinyHouseCar();
            }
            
            // Wait for interval & then spawn next car.
        }

        public string AreaID { get; set; }
        public AreaController AssignedArea { get; set; }

        public TutorialData FirstJobCompletedTutorial => firstJobCompletedTutorial;


        public void InitArea(AreaController area)
        {
            AssignedArea = area;
            AreaID = area.AreaID;
        }

        public Action<TinyHouseCar> OnTinyHouseCarSpawned;
    }
}