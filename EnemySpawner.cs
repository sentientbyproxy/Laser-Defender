using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    public float spawnDelaySeconds = 1f;
    public float spawnDelay = 0.5f;

    private bool movingRight = true;
    private float xmax;
    private float xmin;

    // Use this for initialization
    void Start() {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        xmax = rightBoundary.x;
        xmin = leftBoundary.x;
        SpawnEnemies();
        }
    void SpawnEnemies() {
        foreach (Transform child in transform) {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
            }
        }
    void SpawnUntilFull() {
        Transform freePosition = NextFreePositon();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
            if (NextFreePositon())
            {
                Invoke("SpawnUntilFull", spawnDelaySeconds);
            }
        }
    }
    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
        }

    // Update is called once per frame
    void Update() {
        if (movingRight) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);
        if (leftEdgeOfFormation < xmin) {
            movingRight = true;
        }
        else if (rightEdgeOfFormation > xmax) {
            movingRight = false;
        }
        if (AllMembersDead()) {
            Debug.Log("Empty Formation");
            SpawnUntilFull();
        }
    }
    Transform NextFreePositon() {
        foreach (Transform ChildPositionGameObject in transform) {
            if (ChildPositionGameObject.childCount == 0) {
                return ChildPositionGameObject;
            }
        }
        return null;
    }
        
    bool AllMembersDead() {
        foreach (Transform position in transform) {
            if (position.childCount > 0) {
                return false;
            }
        }
        return true;
    }
}
