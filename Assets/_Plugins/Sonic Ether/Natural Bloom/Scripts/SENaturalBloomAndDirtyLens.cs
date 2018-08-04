using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Sonic Ether/SE Natural Bloom and Dirty Lens")]
public class SENaturalBloomAndDirtyLens : MonoBehaviour
{
	[Range(0.0f, 0.4f)]
	public float bloomIntensity = 0.05f;

	public Shader shader;
	private Material material;

	public Texture2D lensDirtTexture;
	[Range(0.0f, 0.95f)]
	public float lensDirtIntensity = 0.05f;

	private bool isSupported;

	private float blurSize = 4.0f;

	public bool inputIsHDR;

	void Start() 
	{
		isSupported = true;

		if (!material)
			material = new Material(shader);

		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
		{
			isSupported = false;
		}
	}

	void OnDisable()
	{
		if(material)
			DestroyImmediate(material);
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination) 
	{
		if (!isSupported)
		{
			Graphics.Blit(source, destination);
			return;
		}

		if (!material)
			material = new Material(shader);

		#if UNITY_EDITOR
		if (source.format == RenderTextureFormat.ARGBHalf)
			inputIsHDR = true;
		else
			inputIsHDR = false;
		#endif

		material.hideFlags = HideFlags.HideAndDontSave;

		material.SetFloat("_BloomIntensity", Mathf.Exp(bloomIntensity) - 1.0f);
		material.SetFloat("_LensDirtIntensity", Mathf.Exp(lensDirtIntensity) - 1.0f);
		source.filterMode = FilterMode.Bilinear;

		int rtWidth = source.width / 2;
		int rtHeight = source.height / 2;

		RenderTexture downsampled;
		downsampled = source;

		float spread = 1.0f;
		int iterations = 2;

		for (int i = 0; i < 6; i++)
		{
			RenderTexture rt = RenderTexture.GetTemporary(rtWidth, rtHeight, 0, source.format);
			rt.filterMode = FilterMode.Bilinear;

			Graphics.Blit(downsampled, rt, material, 1);

			downsampled = rt;

			if (i > 1)
				spread = 1.0f;
			else
				spread = 0.5f;

			if (i == 2)
				spread = 0.75f;


			for (int j = 0; j < iterations; j++)
			{
				material.SetFloat("_BlurSize", (blurSize * 0.5f + j) * spread);

				//vertical blur
				RenderTexture rt2 = RenderTexture.GetTemporary(rtWidth, rtHeight, 0, source.format);
				rt2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(rt, rt2, material, 2);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;

				rt2 = RenderTexture.GetTemporary(rtWidth, rtHeight, 0, source.format);
				rt2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(rt, rt2, material, 3);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;
			}

			switch (i)
			{
				case 0:
					material.SetTexture("_Bloom0", rt);
					break;
				case 1:
					material.SetTexture("_Bloom1", rt);
					break;
				case 2:
					material.SetTexture("_Bloom2", rt);
					break;
				case 3:
					material.SetTexture("_Bloom3", rt);
					break;
				case 4:
					material.SetTexture("_Bloom4", rt);
					break;
				case 5:
					material.SetTexture("_Bloom5", rt);
					break;
				default: 
					break;
			}

			RenderTexture.ReleaseTemporary(rt);

			rtWidth /= 2;
			rtHeight /= 2;
		}

		material.SetTexture("_LensDirt", lensDirtTexture);
		Graphics.Blit(source, destination, material, 0);
	}
}