using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;


    public class DottedLine : MonoBehaviour
    {
        // Inspector fields
        [SerializeField] GameObject dot;

        [SerializeField] float size;
        [SerializeField] float delta;

        //Static Property with backing field
        private static DottedLine instance;

        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 endPos;
        [SerializeField] private Transform pool; 

        public static DottedLine Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DottedLine>();
                return instance;
            }
        }

        //Utility fields
        List<Vector2> positions = new List<Vector2>();
        static List<GameObject> poolDots = new List<GameObject>();
        List<GameObject> dots = new List<GameObject>();

        // Update is called once per frame
        void FixedUpdate()
        {
            DrawDottedLine(startPos,endPos);
        }

        private void DestroyAllDots()
        {
            foreach (var dot in dots)
            {
                Destroy(dot);
            }
            dots.Clear();
        }

        GameObject GetOneDot()
        {
            GameObject gameObject;

            if (poolDots.Count > 0)
            {
                gameObject = poolDots[poolDots.Count - 1];
                poolDots.RemoveAt(poolDots.Count - 1);
            }
            else {
                gameObject = Instantiate(dot);
            }
          
            gameObject.transform.parent = transform;

            return gameObject;
        }

        public void DrawDottedLine(Vector3 start, Vector3 end)
        {

            float distance = Vector3.Distance(start,end);

            Vector3 direction = (end - start).normalized;       

            int iter = (int)(distance / delta);

            if (dots.Count > iter)
            {
                for (int i = iter; i < dots.Count; ++i)
                {
                    dots[i].SetActive(false);
                    dots[i].transform.parent = pool;
                    poolDots.Add(dots[i]);
                    dots.RemoveAt(i);
                }
            }
            else
            {
                for(int i = dots.Count; i < iter; ++i)
                {
                    dots.Add(GetOneDot());
                }
            }

            for(int i = 0; i < iter; ++i)
            {
                dots[i].transform.position = start + (direction * delta) * i;
            }

            //Vector3 point = start;
            //Vector3 direction = (end - start).normalized;

            //while ((end - start).magnitude > (point - start).magnitude)
            //{
            //    positions.Add(point);
            //    point += (direction * Delta);
            //}

            //Render();
        }

        private void Render()
        {
            foreach (var position in positions)
            {
                var g = GetOneDot();
                g.transform.position = position;
                dots.Add(g);
            }
        }

        public void SetPositions(Vector3 startPosInput , Vector3 endPosInput)
        {
            startPos = startPosInput;
            endPos = endPosInput;
        }
    }
