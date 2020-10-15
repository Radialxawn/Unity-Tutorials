using UnityEngine;

public class PlayerData {
	private static string FName<T>() {
		return typeof(T).FullName;
	}

	public class BestScore {
		public class Mode2D {
			private static readonly string key = FName<Mode2D>(); // key = "PlayerData+BestScore+Mode2D"
			public static int Get() { return PlayerPrefs.GetInt(key, 0); }
			public static void Set(int value) { PlayerPrefs.SetInt(key, value); }
		}

		public class Mode3D {
			private static readonly string key = FName<Mode3D>(); // key = "PlayerData+BestScore+Mode3D"
			public static int Get() { return PlayerPrefs.GetInt(key, 0); }
			public static void Set(int value) { PlayerPrefs.SetInt(key, value); }
		}
	}
}