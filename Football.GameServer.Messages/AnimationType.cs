namespace Football.GameServer.Messages
{
    // Token: 0x02000010 RID: 16
    public enum AnimationType
    {
        // Token: 0x040002B4 RID: 692
        Invalid = -1,
        // Token: 0x040002B5 RID: 693
        FreekickShort,
        // Token: 0x040002B6 RID: 694
        FreekickPass,
        // Token: 0x040002B7 RID: 695
        FreekickLong,
        // Token: 0x040002B8 RID: 696
        PenaltyRight,
        // Token: 0x040002B9 RID: 697
        PenaltyLeft,
        // Token: 0x040002BA RID: 698
        ThrowIn,
        // Token: 0x040002BB RID: 699
        ThrowInShort,
        // Token: 0x040002BC RID: 700
        LongPassRight,
        // Token: 0x040002BD RID: 701
        LongPassLeft,
        // Token: 0x040002BE RID: 702
        LongPassShortRight,
        // Token: 0x040002BF RID: 703
        LongPassShortLeft,
        // Token: 0x040002C0 RID: 704
        PassRight,
        // Token: 0x040002C1 RID: 705
        PassLeft,
        // Token: 0x040002C2 RID: 706
        PassShortRight,
        // Token: 0x040002C3 RID: 707
        PassShortLeft,
        // Token: 0x040002C4 RID: 708
        PassOutsideRight,
        // Token: 0x040002C5 RID: 709
        PassOutsideLeft,
        // Token: 0x040002C6 RID: 710
        PassInterceptRight,
        // Token: 0x040002C7 RID: 711
        PassInterceptLeft,
        // Token: 0x040002C8 RID: 712
        PassHeelRight,
        // Token: 0x040002C9 RID: 713
        PassHeelLeft,
        // Token: 0x040002CA RID: 714
        PassFeintRight,
        // Token: 0x040002CB RID: 715
        PassFeintLeft,
        // Token: 0x040002CC RID: 716
        ShootRight,
        // Token: 0x040002CD RID: 717
        ShootLeft,
        // Token: 0x040002CE RID: 718
        ShootHardRight,
        // Token: 0x040002CF RID: 719
        ShootHardLeft,
        // Token: 0x040002D0 RID: 720
        ShootOutsideRight,
        // Token: 0x040002D1 RID: 721
        ShootOutsideLeft,
        // Token: 0x040002D2 RID: 722
        ShootFullVolleyRight,
        // Token: 0x040002D3 RID: 723
        ShootFullVolleyLeft,
        // Token: 0x040002D4 RID: 724
        ShootTurnRight,
        // Token: 0x040002D5 RID: 725
        ShootTurnLeft,
        // Token: 0x040002D6 RID: 726
        ShootBackLeftFoot,
        // Token: 0x040002D7 RID: 727
        ShootBackRightFoot,
        // Token: 0x040002D8 RID: 728
        VolleyShootTurnLeft,
        // Token: 0x040002D9 RID: 729
        VolleyShootTurnRight,
        // Token: 0x040002DA RID: 730
        VolleyBackLeftFoot,
        // Token: 0x040002DB RID: 731
        VolleyBackRightFoot,
        // Token: 0x040002DC RID: 732
        Fall,
        // Token: 0x040002DD RID: 733
        FallForward,
        // Token: 0x040002DE RID: 734
        FallBack,
        // Token: 0x040002DF RID: 735
        FoulDamageLeft,
        // Token: 0x040002E0 RID: 736
        FoulDamageRight,
        // Token: 0x040002E1 RID: 737
        FoulFall1Left,
        // Token: 0x040002E2 RID: 738
        FoulFall1Right,
        // Token: 0x040002E3 RID: 739
        FoulFall2Left,
        // Token: 0x040002E4 RID: 740
        FoulFall2Right,
        // Token: 0x040002E5 RID: 741
        FoulFall3Left,
        // Token: 0x040002E6 RID: 742
        FoulFall3Right,
        // Token: 0x040002E7 RID: 743
        StumbleRight,
        // Token: 0x040002E8 RID: 744
        StumbleLeft,
        // Token: 0x040002E9 RID: 745
        StumbleHopRight,
        // Token: 0x040002EA RID: 746
        StumbleHopLeft,
        // Token: 0x040002EB RID: 747
        TackleLeft,
        // Token: 0x040002EC RID: 748
        TackleRight,
        // Token: 0x040002ED RID: 749
        FoulTackle1,
        // Token: 0x040002EE RID: 750
        FoulTackle2,
        // Token: 0x040002EF RID: 751
        FoulTackle3,
        // Token: 0x040002F0 RID: 752
        FoulTackle4,
        // Token: 0x040002F1 RID: 753
        FoulTackle5,
        // Token: 0x040002F2 RID: 754
        FoulTackle6,
        // Token: 0x040002F3 RID: 755
        FoulTackle7,
        // Token: 0x040002F4 RID: 756
        FoulTackle8,
        // Token: 0x040002F5 RID: 757
        FoulTackle9,
        // Token: 0x040002F6 RID: 758
        FoulTackle10,
        // Token: 0x040002F7 RID: 759
        FoulTackle11,
        // Token: 0x040002F8 RID: 760
        FoulTackle12,
        // Token: 0x040002F9 RID: 761
        FoulTackle13,
        // Token: 0x040002FA RID: 762
        FoulTackle14,
        // Token: 0x040002FB RID: 763
        FoulTackle15,
        // Token: 0x040002FC RID: 764
        Slide,
        // Token: 0x040002FD RID: 765
        FoulSlide1,
        // Token: 0x040002FE RID: 766
        FoulSlide2,
        // Token: 0x040002FF RID: 767
        FoulSlide3,
        // Token: 0x04000300 RID: 768
        FoulSlide4,
        // Token: 0x04000301 RID: 769
        FoulSlide5,
        // Token: 0x04000302 RID: 770
        FoulSlide6,
        // Token: 0x04000303 RID: 771
        FoulSlide7,
        // Token: 0x04000304 RID: 772
        FoulSlide8,
        // Token: 0x04000305 RID: 773
        FoulSlide9,
        // Token: 0x04000306 RID: 774
        FoulSlide10,
        // Token: 0x04000307 RID: 775
        FoulSlide11,
        // Token: 0x04000308 RID: 776
        FoulSlide12,
        // Token: 0x04000309 RID: 777
        FoulSlide13,
        // Token: 0x0400030A RID: 778
        FoulSlide14,
        // Token: 0x0400030B RID: 779
        FoulSlide15,
        // Token: 0x0400030C RID: 780
        FoulSlide16,
        // Token: 0x0400030D RID: 781
        HeadVolleyPassRight,
        // Token: 0x0400030E RID: 782
        HeadVolleyPassLeft,
        // Token: 0x0400030F RID: 783
        HeadVolleyTurnRight,
        // Token: 0x04000310 RID: 784
        HeadVolleyTurnLeft,
        // Token: 0x04000311 RID: 785
        HeadTurnRight,
        // Token: 0x04000312 RID: 786
        HeadTurnLeft,
        // Token: 0x04000313 RID: 787
        HeadRight,
        // Token: 0x04000314 RID: 788
        HeadLeft,
        // Token: 0x04000315 RID: 789
        HeadFlyRight,
        // Token: 0x04000316 RID: 790
        HeadFlyLeft,
        // Token: 0x04000317 RID: 791
        HeadFlyForward,
        // Token: 0x04000318 RID: 792
        KickFlyForward,
        // Token: 0x04000319 RID: 793
        SlideFlyForward,
        // Token: 0x0400031A RID: 794
        CycleKick,
        // Token: 0x0400031B RID: 795
        CycleKickLow,
        // Token: 0x0400031C RID: 796
        CycleHalfTurnLeft,
        // Token: 0x0400031D RID: 797
        CycleHalfTurnRight,
        // Token: 0x0400031E RID: 798
        ControlChest,
        // Token: 0x0400031F RID: 799
        ControlJumpChest,
        // Token: 0x04000320 RID: 800
        ControlFootLeft,
        // Token: 0x04000321 RID: 801
        ControlFootRight,
        // Token: 0x04000322 RID: 802
        ControlThighLeft,
        // Token: 0x04000323 RID: 803
        ControlThighRight,
        // Token: 0x04000324 RID: 804
        FeintTurnLeft,
        // Token: 0x04000325 RID: 805
        FeintTurnRight,
        // Token: 0x04000326 RID: 806
        HeelUpLeft,
        // Token: 0x04000327 RID: 807
        HeelUpRight,
        // Token: 0x04000328 RID: 808
        JumpWithBallLeft,
        // Token: 0x04000329 RID: 809
        JumpWithBallRight,
        // Token: 0x0400032A RID: 810
        ScissorMoveRight,
        // Token: 0x0400032B RID: 811
        ScissorMoveLeft,
        // Token: 0x0400032C RID: 812
        SpideyBackFlipMove,
        // Token: 0x0400032D RID: 813
        SpideyFastFlipMove,
        // Token: 0x0400032E RID: 814
        WaitLook,
        // Token: 0x0400032F RID: 815
        WaitIdle1,
        // Token: 0x04000330 RID: 816
        WaitIdle2,
        // Token: 0x04000331 RID: 817
        WaitIdle3,
        // Token: 0x04000332 RID: 818
        WaitIdle4,
        // Token: 0x04000333 RID: 819
        WaitIdle5,
        // Token: 0x04000334 RID: 820
        WaitIdle6,
        // Token: 0x04000335 RID: 821
        WaitIdle7,
        // Token: 0x04000336 RID: 822
        WaitIdle8,
        // Token: 0x04000337 RID: 823
        WaitIdle9,
        // Token: 0x04000338 RID: 824
        WaitIdle10,
        // Token: 0x04000339 RID: 825
        WaitIdle11,
        // Token: 0x0400033A RID: 826
        WaitIdle12,
        // Token: 0x0400033B RID: 827
        WaitIdle13,
        // Token: 0x0400033C RID: 828
        WaitIdle14,
        // Token: 0x0400033D RID: 829
        WaitIdle15,
        // Token: 0x0400033E RID: 830
        WaitIdle16,
        // Token: 0x0400033F RID: 831
        WaitIdle17,
        // Token: 0x04000340 RID: 832
        WaitIdle18,
        // Token: 0x04000341 RID: 833
        WaitIdle19,
        // Token: 0x04000342 RID: 834
        WaitIdle20,
        // Token: 0x04000343 RID: 835
        WaitIdle21,
        // Token: 0x04000344 RID: 836
        WaitIdle22,
        // Token: 0x04000345 RID: 837
        WaitIdle23,
        // Token: 0x04000346 RID: 838
        WaitIdle24,
        // Token: 0x04000347 RID: 839
        WaitIdle25,
        // Token: 0x04000348 RID: 840
        WaitIdle26,
        // Token: 0x04000349 RID: 841
        WaitIdle27,
        // Token: 0x0400034A RID: 842
        WaitIdle28,
        // Token: 0x0400034B RID: 843
        WaitIdle29,
        // Token: 0x0400034C RID: 844
        WaitIdle30,
        // Token: 0x0400034D RID: 845
        WalkIdle1,
        // Token: 0x0400034E RID: 846
        WalkIdle2,
        // Token: 0x0400034F RID: 847
        WalkIdle3,
        // Token: 0x04000350 RID: 848
        WalkIdle4,
        // Token: 0x04000351 RID: 849
        WalkIdle5,
        // Token: 0x04000352 RID: 850
        WalkIdle6,
        // Token: 0x04000353 RID: 851
        WalkIdle7,
        // Token: 0x04000354 RID: 852
        WalkIdle8,
        // Token: 0x04000355 RID: 853
        WalkIdle9,
        // Token: 0x04000356 RID: 854
        WalkIdle10,
        // Token: 0x04000357 RID: 855
        WalkIdle11,
        // Token: 0x04000358 RID: 856
        WalkIdle12,
        // Token: 0x04000359 RID: 857
        WalkIdle13,
        // Token: 0x0400035A RID: 858
        WalkIdle14,
        // Token: 0x0400035B RID: 859
        WalkIdle15,
        // Token: 0x0400035C RID: 860
        WalkIdle16,
        // Token: 0x0400035D RID: 861
        WalkIdle17,
        // Token: 0x0400035E RID: 862
        WalkIdle18,
        // Token: 0x0400035F RID: 863
        WalkIdle19,
        // Token: 0x04000360 RID: 864
        WalkIdle20,
        // Token: 0x04000361 RID: 865
        WalkIdle21,
        // Token: 0x04000362 RID: 866
        WalkIdle22,
        // Token: 0x04000363 RID: 867
        WalkIdle23,
        // Token: 0x04000364 RID: 868
        WalkIdle24,
        // Token: 0x04000365 RID: 869
        WalkIdle25,
        // Token: 0x04000366 RID: 870
        WalkIdle26,
        // Token: 0x04000367 RID: 871
        WalkIdle27,
        // Token: 0x04000368 RID: 872
        WalkIdle28,
        // Token: 0x04000369 RID: 873
        WalkIdle29,
        // Token: 0x0400036A RID: 874
        WalkIdle30,
        // Token: 0x0400036B RID: 875
        WalkIdle31,
        // Token: 0x0400036C RID: 876
        WalkIdle32,
        // Token: 0x0400036D RID: 877
        WalkIdle33,
        // Token: 0x0400036E RID: 878
        WalkIdle34,
        // Token: 0x0400036F RID: 879
        WalkIdle35,
        // Token: 0x04000370 RID: 880
        WalkIdle36,
        // Token: 0x04000371 RID: 881
        WalkIdle37,
        // Token: 0x04000372 RID: 882
        WalkIdle38,
        // Token: 0x04000373 RID: 883
        WalkIdle39,
        // Token: 0x04000374 RID: 884
        WalkIdle40,
        // Token: 0x04000375 RID: 885
        WalkIdle41,
        // Token: 0x04000376 RID: 886
        WalkIdle42,
        // Token: 0x04000377 RID: 887
        WalkIdle43,
        // Token: 0x04000378 RID: 888
        WalkIdle44,
        // Token: 0x04000379 RID: 889
        RunStopIdle1,
        // Token: 0x0400037A RID: 890
        RunStopIdle2,
        // Token: 0x0400037B RID: 891
        RunStopIdle3,
        // Token: 0x0400037C RID: 892
        RunStopIdle4,
        // Token: 0x0400037D RID: 893
        RunStopIdle5,
        // Token: 0x0400037E RID: 894
        RunStopIdle6,
        // Token: 0x0400037F RID: 895
        RunStopIdle7,
        // Token: 0x04000380 RID: 896
        RunStopIdle8,
        // Token: 0x04000381 RID: 897
        RunStopIdle9,
        // Token: 0x04000382 RID: 898
        RunStopIdle10,
        // Token: 0x04000383 RID: 899
        RunStopIdle11,
        // Token: 0x04000384 RID: 900
        RunStopIdle12,
        // Token: 0x04000385 RID: 901
        RunStopIdle13,
        // Token: 0x04000386 RID: 902
        RunStopIdle14,
        // Token: 0x04000387 RID: 903
        RunStopIdle15,
        // Token: 0x04000388 RID: 904
        RunStopIdle16,
        // Token: 0x04000389 RID: 905
        RunStopIdle17,
        // Token: 0x0400038A RID: 906
        RunStopIdle18,
        // Token: 0x0400038B RID: 907
        RunStopIdle19,
        // Token: 0x0400038C RID: 908
        RunStopIdle20,
        // Token: 0x0400038D RID: 909
        RunStopIdle21,
        // Token: 0x0400038E RID: 910
        RunStopIdle22,
        // Token: 0x0400038F RID: 911
        RunStopIdle23,
        // Token: 0x04000390 RID: 912
        RunStopIdle24,
        // Token: 0x04000391 RID: 913
        RunStopIdle25,
        // Token: 0x04000392 RID: 914
        RunStopIdle26,
        // Token: 0x04000393 RID: 915
        RunStopIdle27,
        // Token: 0x04000394 RID: 916
        RunStopIdle28,
        // Token: 0x04000395 RID: 917
        RunStopIdle29,
        // Token: 0x04000396 RID: 918
        RunStopIdle30,
        // Token: 0x04000397 RID: 919
        RunStopIdle31,
        // Token: 0x04000398 RID: 920
        RunStopIdle32,
        // Token: 0x04000399 RID: 921
        RunStopIdle33,
        // Token: 0x0400039A RID: 922
        RunStopIdle34,
        // Token: 0x0400039B RID: 923
        RunStopIdle35,
        // Token: 0x0400039C RID: 924
        RunStopIdle36,
        // Token: 0x0400039D RID: 925
        RunStopIdle37,
        // Token: 0x0400039E RID: 926
        RunStopIdle38,
        // Token: 0x0400039F RID: 927
        RunStopIdle39,
        // Token: 0x040003A0 RID: 928
        RunStopIdle40,
        // Token: 0x040003A1 RID: 929
        RunStopIdle41,
        // Token: 0x040003A2 RID: 930
        RunStopIdle42,
        // Token: 0x040003A3 RID: 931
        RunStopIdle43,
        // Token: 0x040003A4 RID: 932
        Cheer1,
        // Token: 0x040003A5 RID: 933
        Cheer2,
        // Token: 0x040003A6 RID: 934
        Cheer3,
        // Token: 0x040003A7 RID: 935
        Cheer4,
        // Token: 0x040003A8 RID: 936
        Cheer5,
        // Token: 0x040003A9 RID: 937
        Cheer6,
        // Token: 0x040003AA RID: 938
        Cheer7,
        // Token: 0x040003AB RID: 939
        TeamCheer1,
        // Token: 0x040003AC RID: 940
        TeamCheer2,
        // Token: 0x040003AD RID: 941
        TeamCheer3,
        // Token: 0x040003AE RID: 942
        TeamCheer4,
        // Token: 0x040003AF RID: 943
        TeamCheer5,
        // Token: 0x040003B0 RID: 944
        ScenarioTeamCheer1,
        // Token: 0x040003B1 RID: 945
        ScenarioTeamCheer2,
        // Token: 0x040003B2 RID: 946
        ScenarioTeamCheer3,
        // Token: 0x040003B3 RID: 947
        ScenarioTeamCheer4,
        // Token: 0x040003B4 RID: 948
        ScenarioTeamCheer5,
        // Token: 0x040003B5 RID: 949
        BonusDance,
        // Token: 0x040003B6 RID: 950
        Hoptek,
        // Token: 0x040003B7 RID: 951
        BonusHeadFlyLeft,
        // Token: 0x040003B8 RID: 952
        BonusHeadFlyRight,
        // Token: 0x040003B9 RID: 953
        BrazilCycleKickLeft,
        // Token: 0x040003BA RID: 954
        BrazilCycleKickRight,
        // Token: 0x040003BB RID: 955
        BrazilShootRight,
        // Token: 0x040003BC RID: 956
        BrazilShootLeft,
        // Token: 0x040003BD RID: 957
        WaitIdle1001,
        // Token: 0x040003BE RID: 958
        WaitIdle1002,
        // Token: 0x040003BF RID: 959
        WaitIdle1003,
        // Token: 0x040003C0 RID: 960
        WaitIdle1004,
        // Token: 0x040003C1 RID: 961
        WaitIdle1005,
        // Token: 0x040003C2 RID: 962
        WaitIdle1006,
        // Token: 0x040003C3 RID: 963
        WaitIdle1007,
        // Token: 0x040003C4 RID: 964
        WaitIdle1008,
        // Token: 0x040003C5 RID: 965
        WaitIdle1009,
        // Token: 0x040003C6 RID: 966
        GkWaitAlert,
        // Token: 0x040003C7 RID: 967
        Freekick1LeftFoot,
        // Token: 0x040003C8 RID: 968
        Freekick1RightFoot,
        // Token: 0x040003C9 RID: 969
        Freekick2LeftFoot,
        // Token: 0x040003CA RID: 970
        Freekick2RightFoot,
        // Token: 0x040003CB RID: 971
        Freekick3LeftFoot,
        // Token: 0x040003CC RID: 972
        Freekick3RightFoot,
        // Token: 0x040003CD RID: 973
        Freekick4LeftFoot,
        // Token: 0x040003CE RID: 974
        Freekick4RightFoot,
        // Token: 0x040003CF RID: 975
        Freekick5LeftFoot,
        // Token: 0x040003D0 RID: 976
        Freekick5RightFoot,
        // Token: 0x040003D1 RID: 977
        Freekick6LeftFoot,
        // Token: 0x040003D2 RID: 978
        Freekick6RightFoot,
        // Token: 0x040003D3 RID: 979
        Freekick7LeftFoot,
        // Token: 0x040003D4 RID: 980
        Freekick7RightFoot,
        // Token: 0x040003D5 RID: 981
        Freekick8LeftFoot,
        // Token: 0x040003D6 RID: 982
        Freekick8RightFoot,
        // Token: 0x040003D7 RID: 983
        Freekick9LeftFoot,
        // Token: 0x040003D8 RID: 984
        Freekick9RightFoot
    }
}
