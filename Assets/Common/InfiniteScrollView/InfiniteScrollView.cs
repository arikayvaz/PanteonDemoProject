using Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common
{
    public class InfiniteScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] ScrollRect scrollRect = null;
        [SerializeField] GridLayoutGroup layoutGroup = null;
        [SerializeField] RectTransform rectTransform = null;
        [SerializeField] float outOfBoundsThreshold = 0.1f;

        public Transform ScrollContentTransform => scrollRect.content.transform;

        Vector2 lastDragPosition;
        bool isSlidingTop;
        int childCount = 0;
        float height = 0f;
        float topPosition = 0f;
        float bottomPosition = 0f;
        private float childHeight = 0f;
        private float itemSpacing = 0f;
        float topThreshold = 0f;
        float bottomThreshold = 0f;

        private void OnEnable()
        {
            scrollRect?.onValueChanged.AddListener(HandleScroll);
        }

        private void OnDisable()
        {
            scrollRect?.onValueChanged.RemoveListener(HandleScroll);
        }

        public void InitScrollView(GameObject goScrollItem, int scrollItemCount, Action<IEnumerable<GameObject>> onComplete) 
        {
            StartCoroutine(StartInitSequence(goScrollItem, scrollItemCount, onComplete));
        }

        private void InitScrollVariables() 
        {
            scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            height = rectTransform.rect.height;
            childCount = scrollRect.content.childCount;

            topPosition = rectTransform.position.y + height * 0.5f;
            bottomPosition = rectTransform.position.y - height * 0.5f;

            itemSpacing = layoutGroup.spacing.y;
            childHeight = (scrollRect.content.GetChild(0).transform as RectTransform).rect.height;

            topThreshold = scrollRect.content.GetChild(0).transform.position.y + outOfBoundsThreshold;
            bottomThreshold = scrollRect.content.GetChild(childCount - 1).transform.position.y - outOfBoundsThreshold;
        }

        private IEnumerator StartInitSequence(GameObject scrollItem, int scrollItemCount, Action<GameObject[]> onComplete)
        {
            layoutGroup.enabled = true;

            yield return new WaitForEndOfFrame();

            GameObject[] spawnedItems = SpawnItems(scrollItem, scrollItemCount);

            yield return new WaitForEndOfFrame();

            layoutGroup.enabled = false;

            InitScrollVariables();

            onComplete?.Invoke(spawnedItems);
        }

        private GameObject[] SpawnItems(GameObject spawnObject, int count)
        {
            GameObject[] items = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                GameObject goItem = Instantiate(spawnObject);
                goItem.transform.SetParent(scrollRect.content.transform);

                goItem.SetActive(true);
                items[i] = goItem;
            }

            return items;
        }

        private void HandleScroll(Vector2 scrollPos) 
        {
            int currentItemIndex = isSlidingTop ? 0 : childCount - 1;
            Transform currentItem = scrollRect.content.GetChild(currentItemIndex);

            if (!IsItemInDesiredPosition(currentItem))
                return;

            int endItemIndex = isSlidingTop ? childCount - 1 : 0;
            Transform endItem = scrollRect.content.GetChild(endItemIndex);
            Vector2 enItemPos = endItem.localPosition;

            Vector2 newPos = enItemPos;

            if (isSlidingTop)
                newPos.y = enItemPos.y - childHeight - itemSpacing;
            else
                newPos.y = enItemPos.y + childHeight + itemSpacing;


            currentItem.localPosition = newPos;
            currentItem.SetSiblingIndex(endItemIndex);
        }

        public void OnBeginDrag(PointerEventData eventData) 
        {
            lastDragPosition = eventData.position;
        }

        const float MOVEMENT_THRESHOLD = 0.01f;
        public void OnDrag(PointerEventData eventData) 
        {
            Vector2 eventPos = eventData.position;

            if (Mathf.Abs(eventPos.y - lastDragPosition.y) < height * MOVEMENT_THRESHOLD)
                return;

            isSlidingTop = eventData.position.y > lastDragPosition.y;
            lastDragPosition = eventData.position;
        }

        private bool IsItemInDesiredPosition(Transform item) 
        {
            return isSlidingTop
                ? item.transform.position.y - childHeight * 0.5f > topThreshold
                : item.transform.position.y + childHeight * 0.5f < bottomThreshold;
        }
    }
}