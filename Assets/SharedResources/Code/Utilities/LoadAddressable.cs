using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NShared
{
	public static class Addressable
	{
		public static async UniTask<TAddressable> Load<TAddressable>(string addressablePath) where TAddressable : Object {
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressablePath);
			handle.ReleaseHandleOnCompletion();
			if (typeof(TAddressable) == typeof(GameObject)) {
				return (await handle) as TAddressable;
			}
			return (await handle).GetComponent<TAddressable>();

		}
	}
}