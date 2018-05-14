using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class OnCollisionEnterSignal : Signal<OnCollisionEnterSignal, Collision>{
		
    }
    
    public class OnCollisionStaySignal : Signal<OnCollisionStaySignal, Collision>{
		
    }
}