using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sigtrap.VrTunnellingPro;


//overrides tunneling file https://assetstore.unity.com/packages/tools/camera/vr-tunnelling-pro-106782
//It simply ensures that the tunneling settings are set to the users selected settings
public class TunnelingCustom : Tunnelling
{
    void Start()
    {
        effectCoverage = StaticLevel.tunnelVisionAmount;
        useAngularVelocity = StaticLevel.turningTunnelVision;
        useAcceleration = StaticLevel.accelerationTunnelVision;
    }

    void Update()
    {
        if(effectCoverage != StaticLevel.tunnelVisionAmount)
        {
            effectCoverage = StaticLevel.tunnelVisionAmount;
        }
        if (useAngularVelocity != StaticLevel.turningTunnelVision)
        {
            useAngularVelocity = StaticLevel.turningTunnelVision;
        }
        if (useAcceleration != StaticLevel.accelerationTunnelVision)
        {
            useAcceleration = StaticLevel.accelerationTunnelVision;
        }
    }

}
