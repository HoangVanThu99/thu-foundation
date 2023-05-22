﻿using System;

// ReSharper disable AccessToStaticMemberViaDerivedType
namespace Pancake.Monetization
{
    [Serializable]
    [EditorIcon("scriptable_variable")]
    public class ApplovinBannerVariable : AdUnitVariable
    {
        public EBannerSize size;
        public EBannerPosition position;

        private bool _isBannerDestroyed = true;
        private bool _registerCallback;

        public override void Load()
        {
#if PANCAKE_ADVERTISING && PANCAKE_APPLOVIN
            if (!_registerCallback)
            {
                MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnAdExpanded;
                MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnAdCollapsed;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                // The latest MAX Unity plugin (versions 4.3.1 and above) enables adaptive banners automatically
                if (size != EBannerSize.Adaptive) MaxSdk.SetBannerExtraParameter(Id, "adaptive_banner", "false");
                _registerCallback = true;
            }

            if (_isBannerDestroyed)
            {
                MaxSdk.CreateBanner(Id, ConvertPosition());
                _isBannerDestroyed = false;
            }
#endif
        }

        public override bool IsReady() { return !string.IsNullOrEmpty(Id); }

        protected override void ShowImpl()
        {
#if PANCAKE_ADVERTISING && PANCAKE_APPLOVIN
            MaxSdk.ShowBanner(Id);
#endif
        }

        public override void Destroy()
        {
#if PANCAKE_ADVERTISING && PANCAKE_APPLOVIN
            if (string.IsNullOrEmpty(Id)) return;
            _isBannerDestroyed = true;
            MaxSdk.DestroyBanner(Id);
#endif
        }

#if PANCAKE_ADVERTISING && PANCAKE_APPLOVIN
        public MaxSdkBase.BannerPosition ConvertPosition()
        {
            switch (position)
            {
                case EBannerPosition.Top: return MaxSdkBase.BannerPosition.TopCenter;
                case EBannerPosition.Bottom: return MaxSdkBase.BannerPosition.BottomCenter;
                case EBannerPosition.TopLeft: return MaxSdkBase.BannerPosition.TopLeft;
                case EBannerPosition.TopRight: return MaxSdkBase.BannerPosition.TopRight;
                case EBannerPosition.BottomLeft: return MaxSdkBase.BannerPosition.BottomLeft;
                case EBannerPosition.BottomRight: return MaxSdkBase.BannerPosition.BottomRight;
                default:
                    return MaxSdkBase.BannerPosition.BottomCenter;
            }
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info) { paidedCallback?.Invoke(info.Revenue, unit, info.NetworkName); }

        private void OnAdCollapsed(string unit, MaxSdkBase.AdInfo info) { C.CallActionClean(ref closedCallback); }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info) { C.CallActionClean(ref faildedToLoadCallback); }

        private void OnAdExpanded(string unit, MaxSdkBase.AdInfo info) { C.CallActionClean(ref displayedCallback); }
        
        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info) { C.CallActionClean(ref loadedCallback); }

#endif
    }
}