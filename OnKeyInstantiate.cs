using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OnKeyInstantiateModule {

    [System.Serializable]
    public class PrefabSlot {

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private Transform targetPoint;

        [SerializeField]
        private Transform parent;

        [SerializeField]
        private float delay;

        /// <summary>
        /// Helper variable used to apply delay between consequent
        /// instantiations.
        /// </summary>
        private float timeToInstantiate;

        public GameObject Prefab {
            get { return prefab; }
            set { prefab = value; }
        }

        public Transform TargetPoint {
            get { return targetPoint; }
            set { targetPoint = value; }
        }

        public Transform Parent {
            get { return parent; }
            set { parent = value; }
        }

        public float Delay {
            get { return delay; }
            set { delay = value; }
        }

        /// <summary>
        /// Helper variable used to apply delay between consequent
        /// instantiations.
        /// </summary>
        public float TimeToInstantiate {
            get { return timeToInstantiate; }
            set { timeToInstantiate = value; }
        }
    }

	public class OnKeyInstantiate : MonoBehaviour {

	    [SerializeField]
	    private List<PrefabSlot> prefabSlots; 

		/// Functions to use to get the key.
		public enum KeyTypes { GetButton, GetKey, GetKeyDown }

		/// Selected function to get the key.
		[SerializeField]
		private KeyTypes _keyType;

		/// Key to instantiate object.
		// TODO Handle also KeyCodes - construct KeyCode from string.
		[SerializeField]
		private string _key;

		public string Key {
			get { return _key; }
			set { _key = value; }
		}

		/// Object to be instantiated.
		[SerializeField]
		private GameObject _gameObject;

		// TODO Add doc
		public GameObject ObjectToInstantiate {
			get { return _gameObject; }
			set { _gameObject = value; }
		}

		/// 3d point where the object should be instantiated.
		[SerializeField]
		private Transform _targetPoint;
		public Transform TargetPoint {
			get { return _targetPoint; }
			set { _targetPoint = value; }
		}

		/// Helper variable.
		///
		/// In-game time to next instantiation.
		private float _nextInstantiation;

		/// Instantiation delay.
		[SerializeField]
		private float _instantiateDelay;

		// TODO Add doc
		public float InstantiateDelay {
			get { return _instantiateDelay; }
			set { _instantiateDelay = value; }
		}

		/// Update function to execute in.
		[SerializeField]
		private UpdateFunctions _updateFunc;

		/// Instantiate without pressing a button.
		[SerializeField]
		private bool _autoInstantiate;

		/// Break after instantiating.
		[SerializeField]
		private bool _breakAtInstatiate;

		// TODO Add doc.
		public bool AutoInstantiate {
			get { return _autoInstantiate; }
			set { _autoInstantiate = value; }
		}

	    public List<PrefabSlot> PrefabSlots {
	        get { return prefabSlots; }
	        set { prefabSlots = value; }
	    }

	    private void Awake() {
			if (!_gameObject) {
                // todo
                //MissingReference("_gameObject", InfoType.Error);
			}
		}

		/// Parent game object for instantiated object.
		///
		/// Leave empty for no parent.
		[SerializeField]
		private Transform _parent;
		
		private void FixedUpdate() {
			if (_updateFunc == UpdateFunctions.FixedUpdate) {
				HandleKeyType();
			}
		}

		private void Update() {
			if (_updateFunc == UpdateFunctions.Update) {
				HandleKeyType();
			}
		}

		private void LateUpdate () {
			if (_updateFunc == UpdateFunctions.LateUpdate) {
				HandleKeyType();
			}
		}

		private void HandleKeyType() {
			switch (_keyType) {
				case KeyTypes.GetButton:
					if (
							// TODO Should use "_key" field.
                            // todo use UniRx
							(Input.GetButton(Key) || _autoInstantiate)) {
						InstantiateObjects();
						if (_breakAtInstatiate) {
							Debug.Break();
						}
					}
					break;
				case KeyTypes.GetKey:
					break;
				case KeyTypes.GetKeyDown:
					if (Input.GetKeyDown(_key)) {
						InstantiateObjects();
						// TODO Execute break condition in each case.
						if (_breakAtInstatiate) {
							Debug.Break();
						}
					}
					break;
			}
		}

	    public void InstantiateObjects() {
	        foreach (var prefabSlot in prefabSlots) {
	            HandleInstantiateObject(prefabSlot);
	        }
	    }

		/// Instantiate new game object.
		private void HandleInstantiateObject(PrefabSlot slot) {
		    if (!TimeToInstantiate(slot)) {
		        return;
		    }

		    // FIXME _projectile (clone) scale is 0.5 the scale of the original.
			var go = (GameObject)Instantiate(
			    slot.Prefab, 
			    slot.TargetPoint.position, 
			    slot.TargetPoint.rotation);

			go.SetActive(true);

			if (slot.Parent) {
				go.transform.parent = slot.Parent.transform;
			}
		}

		/// Return true when it's time to instantiate next object.
		private bool TimeToInstantiate(PrefabSlot slot) {
			if(Time.time > slot.TimeToInstantiate) {
				slot.TimeToInstantiate = Time.time + slot.Delay;
				return true;
			}
			return false;
		}
	}
}
