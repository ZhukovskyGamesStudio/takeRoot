using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Settlers.Test {
    public class PlannedJobView : MonoBehaviour {
        [SerializeField]
        private Transform _lb, _lt, _rt, _rb;

        [SerializeField]
        private SpriteRenderer _jobIcon;

        [SerializeField]
        private GridObject _grid;

        [SerializeField]
        private List<Sprite> _plannedJob;
        [SerializeField]
        private AYellowpaper.SerializedCollections.SerializedDictionary<JobType, Sprite> _jobTypeToSprite;

        [SerializeField]
        private Transform _view;

        private void Start() {
            List<Vector3> corners = _grid.GetObjectCorners();
            _lb.transform.position = new Vector3(corners[0].x, corners[0].y);
            _rb.transform.position = new Vector3(corners[1].x, corners[1].y);
            _rt.transform.position = new Vector3(corners[2].x, corners[2].y);
            _lt.transform.position = new Vector3(corners[3].x, corners[3].y);
            _lb.localScale = new Vector3(1, -1);
            _rt.localScale = new Vector3(-1, 1);
            _rb.localScale = new Vector3(-1, -1);
            _lt.localScale = new Vector3(1, 1);
            _jobIcon.transform.position = _grid.GetObjectCenter();
            gameObject.transform.position = _grid.transform.position;
        }

        public void Enable(JobType jobType) {
            _jobIcon.sprite = _jobTypeToSprite[jobType];
            gameObject.SetActive(true);
        }
    }
}