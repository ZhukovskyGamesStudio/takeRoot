using System.Collections.Generic;
using UnityEngine;

public interface ITacticalService : IService {
	public void SetTacticalForSelectedSettlers(bool isOn);
}