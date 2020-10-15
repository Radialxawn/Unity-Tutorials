using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
	#region Float
	public static float Snap(this float self, float grid) {
		int div = (int)(self / grid);
		int mod = (self - div * grid) >= grid * 0.5f ? 1 : 0;
		return grid * (div + mod);
	}

	public static float Clamp(this float self, float min, float max) {
		return self < min ? min : self > max ? max : self;
	}
	#endregion

	#region Vector3
	public static Vector3 SetX(this Vector3 self, float x) { self.x = x; return self; }
	public static Vector3 SetY(this Vector3 self, float y) { self.y = y; return self; }
	public static Vector3 SetZ(this Vector3 self, float z) { self.z = z; return self; }

	public static Vector3 To(this Vector3 self, Vector3 target) {
		target.x -= self.x; target.y -= self.y; target.z -= self.z;
		return target;
	}
	#endregion
}