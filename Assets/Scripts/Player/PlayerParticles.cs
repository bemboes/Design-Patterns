using UnityEngine;
using System.Collections;

/// <summary>
/// A particles class that spawns Particle prefabs on the player.
/// </summary>
public class PlayerParticles : MonoBehaviour
{
    [SerializeField]
    private GameObject shockwavePrefab;
    [SerializeField]
    private GameObject runFogPrefab;
    [SerializeField]
    private GameObject glistenPrefab;

    private GameObject runFogInstance;

    private float timeStampLastShockWave = 0;

    private void Start ()
    {
        runFogInstance = GameObject.Instantiate(runFogPrefab, transform.position, runFogPrefab.transform.rotation) as GameObject;
        runFogInstance.transform.parent = transform;
        runFogInstance.transform.rotation = runFogPrefab.transform.rotation;
    }

    /// <summary>
    /// Creates a running fog that follows the player. (Needs upgrade for SpeedupPowerup).
    /// </summary>
    /// <param name="active">The new active state of the running fog.</param>
    public void SetRunFog(bool active)
    {
        if (!active)
        {
            // Uncouple the running Particles from the player
            // And stop the emission system.

            // Uncouple from the player transform (stop following).
            runFogInstance.transform.SetParent(null);

            // Add the selfdestruct script
            SelfDestruct sd = runFogInstance.GetComponent<SelfDestruct>();
            sd.enabled = true;

            // Set fade-out particle system behaviour
            ParticleSystem ps = runFogInstance.GetComponent<ParticleSystem>();
            ps.loop = false;
            ps.Stop();

            // Destroy reference so new fog can follow the player.
            runFogInstance = null;
        }
        else
        {
            // If no Fog is following the player, create a new particle system.
            if(runFogInstance == null)
            {
                runFogInstance = GameObject.Instantiate(runFogPrefab, transform.position, runFogPrefab.transform.rotation) as GameObject;
                runFogInstance.transform.parent = transform;
                runFogInstance.transform.rotation = runFogPrefab.transform.rotation;
            }
        }
    }

    /// <summary>
    /// Creates a shockwave when the player hits the floor (at pos).
    /// </summary>
    /// <param name="isJumpRolling">Whether the player was fast-falling (jumpDashing/jumpRolling).</param>
    /// <param name="pos">Point of impact with the collider and the floor.</param>
    public void CreateShockwave(bool isJumpRolling, Vector3 pos)
    {
        // If enough time has passed, create new shockwave
        // (Dirty fix for ShockWave creation being called twice in a row.)
        if (Time.realtimeSinceStartup - timeStampLastShockWave > 0.4f)
        {
            // Create object
            GameObject go = (GameObject) GameObject.Instantiate(shockwavePrefab, pos + shockwavePrefab.transform.localPosition, shockwavePrefab.transform.rotation);
            timeStampLastShockWave = Time.realtimeSinceStartup;

            // Make the shockwave smaller if the player was jumping regularly.
            if(!isJumpRolling)
            {
                ParticleSystem ps = go.GetComponent<ParticleSystem>();
                ps.startColor = new Color(1, 1, 1, 0.1f);
                ps.startSize = 0.4f;
            }
        }
    }

    /// <summary>
    /// Creates a series of glistening lines around the player (eg. when powerup is pickedup).
    /// </summary>
    public void CreateGlisten()
    {
        GameObject go = (GameObject) GameObject.Instantiate(glistenPrefab, transform.position, glistenPrefab.transform.rotation);

        // Make the glisten a child of the player (follow the player).
        go.transform.parent = transform;
    }
}
