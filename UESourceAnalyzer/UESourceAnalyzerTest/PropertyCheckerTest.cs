using NUnit.Framework;
using System;
using UESourceAnalyzer.PropertyCheck;

namespace UESourceAnalyzerTest
{
    public class PropertyCheckerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsValidTest()
        {
            var propertyChecker = new PropertyChecker();
            var lines = TestCode.Split(Environment.NewLine);

            for (var i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];

                if (!propertyChecker.IsMatch(i, lines))
                {
                    if (line.Contains("NG"))
                    {
                    }

                    continue;
                }

                propertyChecker = new PropertyChecker();
                if (line.Contains("OK"))
                {
                    Assert.IsTrue(propertyChecker.IsValid(i, lines));
                }
                if (line.Contains("NG"))
                {
                    Assert.IsFalse(propertyChecker.IsValid(i, lines));
                }
            }
        }

        readonly string TestCode = @"#pragama once

#include ""UProperty.h""
#include <UProperty.h>

UCLASS()
class UPROPERTY_API UProperty : public UObject
{
    GENERATED_BODY()

    UHoge* NG;

    UPROPERTY()
        UHoge* OK;

    UPROPERTY() UHoge* OK;

    UFUNCTION() UHoge* OK();
    UFUNCTION() UHoge* OK() const;
    UFUNCTION() 
        UHoge* OK();
    UFUNCTION() 
        UHoge* OK() const;
    
    FHoge* OK;
    UPROPERTY() FHoge* OK;

    // UHoge* OK;
    /* UHoge* OK; */
    /* 
    UHoge* OK; 
    */
    /* 



    UHoge* OK; 



    */

    // UPROPERTY()
        UHoge* NG;

    /* UPROPERTY() */
        UHoge* NG;

    UPROPERTY() 
        // Hogehoge
        UHoge* OK;
    UPROPERTY() 
        /* Hogehoge */
        UHoge* OK;

    /* UPROPERTY() */ UHoge* NG;

    TArray<UHoge*> NG;
    
    UPROPERTY()
        TArray<UHoge*> OK;

    TArray<FHoge*> OK;
    
    TMap<FHoge, UHoge*> NG;
    UPROPERTY()
        TMap<FHoge, UHoge*> OK;

    AHoge* NG;

    UPROPERTY()
        AHoge* OK;

    UPROPERTY() AHoge* OK;

    UFUNCTION() AHoge* OK();
    UFUNCTION() AHoge* OK() const;
    UFUNCTION() 
        AHoge* OK();
    UFUNCTION() 
        AHoge* OK() const;
    
    FHoge* OK;
    UPROPERTY() FHoge* OK;

    // AHoge* OK;
    /* AHoge* OK; */
    /* 
    AHoge* OK; 
    */
    /* 



    AHoge* OK; 



    */

    // UPROPERTY()
        AHoge* NG;

    /* UPROPERTY() */
        AHoge* NG;

    /* UPROPERTY() */ AHoge* NG;

    TArray<AHoge*> NG;
    
    UPROPERTY()
        TArray<AHoge*> OK;

    TArray<FHoge*> OK;
    
    TMap<FHoge, AHoge*> NG;
    UPROPERTY()
        TMap<FHoge, AHoge*> OK;

};";
    }
}