using System.Collections;
using PuzzleRun;
using UnityEngine;

namespace Minigames.FlyingHazard.Scripts
{
    public class Disappear : MonoBehaviour
    {
        private SpriteRenderer sr;
        public void Start()
        {
            // Destroy(ga, 5f);
            sr = GetComponent<SpriteRenderer>();

            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSeconds(5f);
            for (float t = 0f; t < 1f; t += Time.deltaTime)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1-t);
                yield return null;
            }

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        }
    }
}