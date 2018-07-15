using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoiseHUDDisplay : MonoBehaviour
{
    [SerializeField] Canvas Canvas;
    [SerializeField] GameObject FootstepsIconPrefab;

    HearingEntity _hearingEntity;
    List<GameObject> _images;

    void Awake()
    {
        _images = new List<GameObject>();
    }

    void Update()
    {
        _images.ForEach(i => i.gameObject.SetActive(false));

        foreach (var noisePos in GetPlayerHearingEntity().NoiseLocations)
        {
            var canvasPos = Camera.main.WorldToScreenPoint(noisePos);
            var image = GetFreeImage();
            image.GetComponent<RectTransform>()
                .anchoredPosition = canvasPos;
            image.gameObject.SetActive(true);
        }
    }

    HearingEntity GetPlayerHearingEntity()
    {
        if (_hearingEntity == null)
        {
            _hearingEntity = FindObjectOfType<Player>()
                   .GetComponent<EntityContainer>()
                   .GetEntity<HearingEntity>();
        }
        return _hearingEntity;
    }

    RectTransform GetFreeImage()
    {
        var imageGo = _images.FirstOrDefault(i => !i.activeSelf);
        if (imageGo)
        {
            return imageGo.GetComponent<RectTransform>();
        }

        imageGo = Instantiate(FootstepsIconPrefab, Canvas.transform);
        _images.Add(imageGo);
		return imageGo.GetComponent<RectTransform>();
    }
}
