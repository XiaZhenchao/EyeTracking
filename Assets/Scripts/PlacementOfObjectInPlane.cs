using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class PlacementOfObjectInPlane : MonoBehaviour
{
    [SerializeField]
   private GameObject placedPrefab;
   private GameObject placeObject;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    int NumberOfPlane = 0;
    ARPlane arPlane;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if (NumberOfPlane < 1)
        {
            arPlaneManager.planesChanged += PlaneChanged;
            ++NumberOfPlane;
        }
    }

    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        //ARPlanesChangedEventArgs -struct
        //public ARPlanesChangedEventArgs(List<ARPlane> added, List<ARPlane> updated, List<ARPlane> removed
        if (NumberOfPlane < 2)
        {
            arPlane = args.added[0];
            ++NumberOfPlane;
        

        Vector3 PlanePosition = arPlane.transform.position;
            for (int i = 0; i < 18; i++)
            {
                placeObject = Instantiate(placedPrefab, PlanePosition, Quaternion.identity);
                PlanePosition = arPlane.transform.position + new Vector3(0f, 0f, i * 0.3f);
            }

            for (int i = 0; i < 18; i++)
            {
                PlanePosition = arPlane.transform.position + new Vector3(i * 0.3f, 0f, 5.1f);
                placeObject = Instantiate(placedPrefab, PlanePosition, Quaternion.identity);
            }

            PlanePosition = arPlane.transform.position;

            for (int i = 0; i < 18; i++)
            {
                PlanePosition = arPlane.transform.position + new Vector3(i * 0.3f, 0f, 0f);
                placeObject = Instantiate(placedPrefab, PlanePosition, Quaternion.identity);
            }

            for (int i = 0; i < 18; i++)
            {
                placeObject = Instantiate(placedPrefab, PlanePosition, Quaternion.identity);
                PlanePosition = arPlane.transform.position + new Vector3(5.1f, 0f, i * 0.3f);
            }



            //public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
            //Quaternion rotation means no rotation
        }
    }
     

}
