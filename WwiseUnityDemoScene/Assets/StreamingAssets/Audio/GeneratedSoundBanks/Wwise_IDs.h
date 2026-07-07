/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID FOOTSTEP = 1866025847U;
        static const AkUniqueID PLAY_BASICTEST = 2107159840U;
        static const AkUniqueID PLAY_MUSICSEGMENT = 2524914122U;
        static const AkUniqueID PLAYCUBEMOVEMENT = 301909423U;
        static const AkUniqueID PLAYDIALOGUE = 2341029961U;
        static const AkUniqueID PLAYHELLO = 4862705U;
        static const AkUniqueID PLAYIMPACT = 3660193989U;
        static const AkUniqueID PLAYIMPACTHIGH = 2496117889U;
        static const AkUniqueID PLAYIMPACTLOW = 1544558053U;
        static const AkUniqueID PLAYROOMTONE = 2570281838U;
        static const AkUniqueID PLAYSUBTITLES = 3875076402U;
        static const AkUniqueID USERDEFINEDEVENT = 2473335609U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace TIMEOFDAY
        {
            static const AkUniqueID GROUP = 3729505769U;

            namespace STATE
            {
                static const AkUniqueID DAY = 311764537U;
                static const AkUniqueID NIGHT = 1011622525U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace TIMEOFDAY

    } // namespace STATES

    namespace SWITCHES
    {
        namespace FOOTSTEP_MATERIAL
        {
            static const AkUniqueID GROUP = 684570577U;

            namespace SWITCH
            {
                static const AkUniqueID DIRT = 2195636714U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID GRAVEL = 2185786256U;
                static const AkUniqueID NOTHING = 4248742144U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace FOOTSTEP_MATERIAL

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID CUBEACCELERATION = 501015974U;
        static const AkUniqueID TESTCURRENTVOLUME = 772397858U;
    } // namespace GAME_PARAMETERS

    namespace TRIGGERS
    {
        static const AkUniqueID STINGER = 78360149U;
    } // namespace TRIGGERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID DLC = 529873620U;
        static const AkUniqueID TEST_SOUNDBANK = 4154996803U;
        static const AkUniqueID USERDEFINEDSOUNDBANK = 2656110914U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTER_SECONDARY_BUS = 805203703U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID BLUECAVE = 3050928690U;
        static const AkUniqueID LARGEROOM = 187046019U;
        static const AkUniqueID REDCAVE = 3822919377U;
        static const AkUniqueID SMALLROOM = 2933838247U;
        static const AkUniqueID THIRDPERSON = 2763016207U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID AUXILIARY = 2396789687U;
        static const AkUniqueID COMMUNICATION = 530303819U;
        static const AkUniqueID CONTROLLER_HEADPHONES = 2868300805U;
        static const AkUniqueID CONTROLLER_SPEAKER = 1334442663U;
        static const AkUniqueID DVR = 697649671U;
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
