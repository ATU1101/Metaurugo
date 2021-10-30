using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Blockbreak : MonoBehaviour
{



    private void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D ot)
    {
        var hitPos = Vector3.zero;

        foreach (var point in ot.contacts)
        {
            hitPos = point.point;
        }

        var position = ot.gameObject.GetComponent<Tilemap>().cellBounds.allPositionsWithin;
        var minPosition = 0;
        var allPosition = new List<Vector3>();

        foreach (var variable in position)
        {
            if (ot.gameObject.GetComponent<Tilemap>().GetTile(variable) != null)
            {
                allPosition.Add(variable);
                Debug.Log(variable.ToString());
            }
        }

        for (var i = 1; i < allPosition.Count; i++)
        {
            if ((hitPos - allPosition[i]).magnitude <
                (hitPos - allPosition[minPosition]).magnitude)
            {
                minPosition = i;
            }
        }

        var finalPosition = Vector3Int.RoundToInt(allPosition[minPosition]);

        var tiletmp = ot.gameObject.GetComponent<Tilemap>().GetTile(finalPosition);

        if (tiletmp != null)
        {
            var map = ot.gameObject.GetComponent<Tilemap>();
            var tileCol = ot.gameObject.GetComponent<TilemapCollider2D>();
            map.SetTile(finalPosition, null);
            tileCol.enabled = false;
            tileCol.enabled = true;

        }
    }
}
