using System;
using UnityEngine;
using UnityEngine.Analytics;
using Zenject;

namespace AnotherTankGame{
	
	public class ChangeGameStateSignal : Signal<ChangeGameStateSignal, GameStates, GameOverType>{
		
	}

	public class CollidedWithBlock : Signal<CollidedWithBlock, Collision>{
		
	}
	
	public class BulletIsDespawnedSignal : Signal<BulletIsDespawnedSignal>{
		
	}

	public class StartGameSignal : Signal<StartGameSignal>{
		
	}

	public class GamePausedSignal : Signal<GamePausedSignal>{
		
	}

	public class ResumeGameSignal : Signal<ResumeGameSignal>{
		
	}

	public class GameOverSignal : Signal<GameOverSignal, GameOverType>{
		
	}
	
	public class WaitingToStartSignal : Signal<WaitingToStartSignal>{
		
	}
	
	public class DebugSignal : Signal<DebugSignal>{
		
	}
	
	public class SetLevelNameSignal : Signal<SetLevelNameSignal, string>{
		
	}
	
	public class SetPlayerLifesSignal : Signal<SetPlayerLifesSignal, int>{
		
	}
	
	public class SetEnemyTankCountSignal : Signal<SetEnemyTankCountSignal, EnemyTankCount >{
		
	}

	public class OnBaseExplodedSignal : Signal<OnBaseExplodedSignal>{
		
	}
	
	public class OnBrickHitSignal : Signal<OnBrickHitSignal>{
		
	}
	
	public class OnWallHitSignal : Signal<OnWallHitSignal>{
		
	}

	public class OnPlayerExplodedSignal : Signal<OnPlayerExplodedSignal>{
		
	}
	
	public class OnPlayerShootSignal : Signal<OnPlayerShootSignal>{
		
	}
	
	public class OnEnemyExplodedSignal : Signal<OnEnemyExplodedSignal>{
		
	}

	public class OnSFXVolChangedSignal : Signal<OnSFXVolChangedSignal, float>{
		
	}
	
	public class OnMusicVolChangedSignal : Signal<OnMusicVolChangedSignal, float>{
		
	}

	public class OnLevelSelectedSignal : Signal<OnLevelSelectedSignal, int>{
		
	}
}
