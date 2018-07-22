using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoiseHUDDisplay : MonoBehaviour
{
    [SerializeField] Canvas Canvas;
    [SerializeField] GameObject IconPrefab;
    [SerializeField] Sprite WalkingSprite;
    [SerializeField] Sprite RoarSprite;
    [SerializeField] Sprite ClickerScreamSprite;

    HearingEntity _hearingEntity;
    List<GameObject> _images;

    void Awake()
    {
        _images = new List<GameObject>();
    }

    void Update()
    {
        _images.ForEach(i => i.gameObject.SetActive(false));

        foreach (var noiseEntity in GetPlayerHearingEntity().NoiseEntities)
        {
            bool isSpotted = noiseEntity.GetEntity<SpottableEntity>().Spotted;
            if (isSpotted)
            {
                continue;
            }
            var noise = noiseEntity.GetEntity<NoiseProducerEntity>();
            var noisePos = noiseEntity.GetEntity<IPhysicalEntity>()
                .Position;
            var canvasPos = Camera.main.WorldToScreenPoint(noisePos);
            var image = GetFreeImage(noise.Type);
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

    RectTransform GetFreeImage(NoiseType type)
    {
        var imageGo = _images.FirstOrDefault(i => !i.activeSelf);
        if (!imageGo)
        {
            imageGo = Instantiate(IconPrefab, Canvas.transform);
            _images.Add(imageGo);
        }
        var image = imageGo.GetComponent<Image>();
        switch (type)
        {
            case NoiseType.Footsteps:
                {
                    image.sprite = WalkingSprite;
                    break;
                }
            case NoiseType.Roar:
                {
                    image.sprite = RoarSprite;
                    break;
                }
            case NoiseType.ClickerScream:
                {
                    image.sprite = ClickerScreamSprite;
                    break;
                }
            default:
                break;
        }
        return imageGo.GetComponent<RectTransform>();
    }
}
