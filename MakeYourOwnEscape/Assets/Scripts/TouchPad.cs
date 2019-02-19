using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Image))]
public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	// Options for which axes to use
	public enum AxisOption
	{
		Both, // Use both
		OnlyHorizontal, // Only horizontal
		OnlyVertical // Only vertical
	}

	public enum ControlStyle
	{
		Absolute, // operates from teh center of the image
		Relative, // operates from the center of the initial touch
		Swipe, // swipe to touch touch no maintained center
	}

    #region Script Parameters
    public float StickRadius;
    public Transform BaseImage;
    public Transform StickImage;
    public AxisOption AxesToUse = AxisOption.Both; // The options for the axes that the still will use
	public ControlStyle Style = ControlStyle.Absolute; // control style to use
	public string HorizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
	public string VerticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
    public string ButtonName;
    public float Xsensitivity = 1f;
	public float Ysensitivity = 1f;
    #endregion

    #region Fields
    Vector3 mStartPos;
	Vector2 mPreviousDelta;
	Vector3 mJoytickOutput;
	bool mUseX; // Toggle for using the x axis
	bool mUseY; // Toggle for using the Y axis
	CrossPlatformInputManager.VirtualAxis mHorizontalVirtualAxis; // Reference to the joystick in the cross platform input
	CrossPlatformInputManager.VirtualAxis mVerticalVirtualAxis; // Reference to the joystick in the cross platform input
	bool mDragging;
	int mId = -1;
	Vector2 mPreviousTouchPos; // swipe style control touch
    private Vector3 mCenter;
    private Image mImage;
    #endregion

    #region Unity Methods
    void OnEnable()
	{
		CreateVirtualAxes();
	}

    void Start()
    {
        BaseImage.gameObject.SetActive(false);
        mImage = GetComponent<Image>();
        mCenter = mImage.transform.position;
    }
    #endregion

    #region Methods
    public void OnPointerDown(PointerEventData data)
    {
        CrossPlatformInputManager.SetButtonDown(ButtonName);
        BaseImage.gameObject.SetActive(true);
        BaseImage.position = data.position;
        mDragging = true;
        mId = data.pointerId;
        if(Style != ControlStyle.Absolute)
        {
            mCenter = data.position;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        CrossPlatformInputManager.SetButtonUp(ButtonName);
        BaseImage.gameObject.SetActive(false);
        mDragging = false;
        mId = -1;
        //UpdateVirtualAxes(Vector3.zero);
    }
    #endregion

    #region Implementation
    void CreateVirtualAxes()
	{
		// set axes to use
		mUseX = (AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyHorizontal);
		mUseY = (AxesToUse == AxisOption.Both || AxesToUse == AxisOption.OnlyVertical);

		// create new axes based on axes to use
		if(mUseX)
		{
			mHorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(HorizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(mHorizontalVirtualAxis);
		}
		if(mUseY)
		{
			mVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VerticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(mVerticalVirtualAxis);
		}
	}

	void UpdateVirtualAxes(Vector3 value)
	{
		//value = value.normalized;
		if(mUseX)
		{
			mHorizontalVirtualAxis.Update(value.x);
		}

		if(mUseY)
		{
			mVerticalVirtualAxis.Update(value.y);
		}
	}
    
    void Update()
	{
		if(!mDragging)
		{
			return;
		}
		if(Input.touchCount >= mId + 1 && mId != -1)
		{
#if !UNITY_EDITOR
            if(Style == ControlStyle.Swipe)
            {
                mCenter = mPreviousTouchPos;
                mPreviousTouchPos = Input.touches[mId].position;
            }
            Vector2 pointerDelta = new Vector2(Input.touches[mId].position.x - mCenter.x , Input.touches[mId].position.y - mCenter.y);
            pointerDelta.x *= Xsensitivity;
            pointerDelta.y *= Ysensitivity;
#else
			Vector2 pointerDelta;
			pointerDelta.x = Input.mousePosition.x - mCenter.x;
			pointerDelta.y = Input.mousePosition.y - mCenter.y;
            if(Style == ControlStyle.Absolute)
            {
			    mCenter = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            }
#endif

            var stickDistance = pointerDelta / StickRadius;
            if(stickDistance.sqrMagnitude > 1f)
            {
                stickDistance.Normalize();
            }
            StickImage.localPosition = stickDistance * StickRadius;
            UpdateVirtualAxes(stickDistance);
		}
	}
    
	void OnDisable()
	{
        if(CrossPlatformInputManager.AxisExists(HorizontalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(HorizontalAxisName);
        }

		if(CrossPlatformInputManager.AxisExists(VerticalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(VerticalAxisName);
        }
    }
    #endregion
}
