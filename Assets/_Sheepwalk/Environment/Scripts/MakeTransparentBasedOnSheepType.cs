using System.Collections;
using System.Collections.Generic;
using sheepwalk;
using UnityEngine;

public class MakeTransparentBasedOnSheepType : MonoBehaviour
{
    [SerializeField] private float updateDelay = 2f;

    [SerializeField] private string tagForVisible = "";
    [SerializeField] private Texture transparentTexture;
    [SerializeField] private Texture fullyVisibleTexture;
    
    private Renderer _renderer;

    private float transparencyValue = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        StartCoroutine(nameof(Timer));
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateDelay);
            RareUpdate();
        }
    }

    private void RareUpdate()
    {
        var player = PlayerReference.PlayerController;
        if (player == null || player.Pawn == null)
        {
            SetTransparency(transparencyValue);
            return;
        }
        var tags = player.Pawn.GetComponent<CustomTags>();
        // 0 or 1?
        if (tags.HasTag(tagForVisible)) SetTransparency(1f);
        else SetTransparency(transparencyValue);
        
    }

    private void SetTransparency(float value)
    {
        if (value < 1f)
        {
            _renderer.material.mainTexture = transparentTexture;
            _renderer.material.SetFloat("_Mode", 1);
        }
        else
        {
            _renderer.material.mainTexture = fullyVisibleTexture;
            _renderer.material.SetFloat("_Mode", 0);
        }
        
    }
    
}
