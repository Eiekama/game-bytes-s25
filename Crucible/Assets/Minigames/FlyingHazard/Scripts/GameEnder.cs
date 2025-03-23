using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameEnder : MonoBehaviour
{
    public GameObject bird1;
    public GameObject bird2;

    private GameObject music;
    BirdScript BirdScript1;

    BirdScript BirdScript2;

    Minigames.FlyingHazard.Scripts.Player p1;

    Minigames.FlyingHazard.Scripts.Player p2;
    
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        music = GameObject.Find("Music Player");
        audio = music.GetComponent<AudioSource>();
        BirdScript1 = bird1.GetComponent<BirdScript>();
        BirdScript2 = bird2.GetComponent<BirdScript>();
        p1 = bird1.GetComponent<Minigames.FlyingHazard.Scripts.Player>();
        p2 = bird2.GetComponent<Minigames.FlyingHazard.Scripts.Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (p1.getLives() <= 0 && p2.getLives() <= 0){
            if(p1.Score == p2.Score)
            {
                MinigameController.Instance.FinishGame(LastMinigameFinish.TIE);
            }
            else
            {
                MinigameController.Instance.FinishGame(p1.Score > p2.Score ? LastMinigameFinish.P1WIN : LastMinigameFinish.P2WIN);
            }
            StartCoroutine(stopMusic());
        }    
    }

    private IEnumerator stopMusic(){
        yield return new WaitForSeconds(2f);
        audio.mute = true;
        /*while (audio.volume > 0){
            yield return new WaitForSeconds(1f);
            audio.volume -= 0.01f;
        }*/
        //Was trying to make the music fade out but it wasn't working and not sure why
    }
}
