using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITacticalService : IService {
	
	public Action OnTacticalChanged { get; set; }
	public void SetTacticalForSelectedSettlers(bool isOn);
}