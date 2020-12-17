using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelManager : MonoBehaviour
{
    [SerializeField] private Parcel parcelTemplate;
    [SerializeField] private float uniqueParcelDropChance = 0.1f;
    [SerializeField] private List<Parcel> uniqueParcels;
    [SerializeField] private string[] randomFrom;
    [SerializeField] private string[] randomNote;
    [SerializeField] private string[] randomWaitingForDialog;
    [SerializeField] private string[] randomRecievedDialog;
    [SerializeField] private Sprite[] randomParcelSprite;
    [SerializeField] private Sprite[] randomParcelSpriteHighlighted;

    private List<Parcel> _activeParcels;
    private List<Parcel> _inactiveParcels;
    private List<Parcel> _pooledParcels;
    private Player _player;
    private Bounds _bounds;

    void Awake()
    {
        _activeParcels = new List<Parcel>();
        _inactiveParcels = new List<Parcel>();
        _pooledParcels = new List<Parcel>();
        Collider2D deliveryArea = GetComponent<Collider2D>();
        _bounds = deliveryArea.bounds;
        foreach(Parcel uniqueParcel in uniqueParcels)
        {
            uniqueParcel.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            GenerateNewParcel();
        }
        _player = GameLogic.GetInstance().GetPlayer();
    }

    void Update()
    {
        if(!_player.IsHoldingParcel())
        {
            _player.HighlightNearbyParcel(_activeParcels);
        }
        // Possibly remove this? It's funnier when they stack up.
        // for(int i = _inactiveParcels.Count - 1; i >= 0; i--)
        // {
        //     if(!_inactiveParcels[i].IsVisible())
        //     {
        //         Parcel p = _inactiveParcels[i];
        //         _inactiveParcels.RemoveAt(i);
        //         p.gameObject.SetActive(false);
        //         _pooledParcels.Add(p);
        //     }
        // }
    }

    internal bool ExpectingParcel(NPC deliverTo, out Parcel expectedParcel)
    {
        foreach(Parcel parcel in _activeParcels)
        {
            if(parcel.deliverTo == deliverTo)
            {
                expectedParcel = parcel;
                return true;
            }
        }
        expectedParcel = null;
        return false;
    }

    internal void SetParcelInactive(Parcel parcel)
    {
        _activeParcels.Remove(parcel);
        _inactiveParcels.Add(parcel);
    }

    public void GenerateNewParcel()
    {
        Parcel newParcel;
        if(Random.value < uniqueParcelDropChance && uniqueParcels.Count > 0)
        {
            int index = Random.Range(0, uniqueParcels.Count);
            newParcel = uniqueParcels[index];
            uniqueParcels.RemoveAt(index);
        }
        else
        {
            if(_pooledParcels.Count > 0)
            {
                int lastIndex = _pooledParcels.Count - 1;
                newParcel = _pooledParcels[lastIndex];
                _pooledParcels.RemoveAt(lastIndex);
            }
            else
            {
                newParcel = GameObject.Instantiate<Parcel>(parcelTemplate);
            }

            newParcel.deliverTo = GameLogic.GetInstance().GetRandomNPC();
            newParcel.from = randomFrom[Random.Range(0, randomFrom.Length)];
            newParcel.note = randomNote[Random.Range(0, randomNote.Length)];
            newParcel.waitingForDialog = randomWaitingForDialog[Random.Range(0, randomWaitingForDialog.Length)];
            newParcel.recievedDialog = randomRecievedDialog[Random.Range(0, randomRecievedDialog.Length)];
            newParcel.waitingForDialog = newParcel.waitingForDialog.Replace("<name>", newParcel.from);
            newParcel.recievedDialog = newParcel.recievedDialog.Replace("<name>", newParcel.from);

            int spriteIndex = Random.Range(0, randomParcelSprite.Length);
            newParcel.SetParcelSprites(randomParcelSprite[spriteIndex], randomParcelSpriteHighlighted[spriteIndex]);
        }
        _activeParcels.Add(newParcel);
        newParcel.gameObject.SetActive(true);
        float positionX = Random.Range(_bounds.min.x, _bounds.max.x);
        float positionY = Random.Range(_bounds.min.y, _bounds.max.y);
        newParcel.transform.position = new Vector3(positionX, positionY, 0f);
    }
}
