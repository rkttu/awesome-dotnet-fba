#!/usr/bin/env dotnet

#:property PublishAot=False

// IKVM Maven SDK integration - allows direct usage of Maven packages in .NET
#:package IKVM.Maven.Sdk@1.9.4

// Import Korean text processing utilities from different Java libraries
using KkmaHangul = org.snu.ids.ha.util.Hangul;  // KKMA (Korean Knowledge Morpheme Analyzer)
using OktHangul = org.openkoreantext.processor.util.Hangul;  // Open Korean Text processor

// Example 1: Using KKMA to decompose Korean character
var kkma = new KkmaHangulUtil();

// Split the Korean character '한' (han) into its constituent parts
kkma.Split('\uD55C', out var initial, out var medial, out var final);

Console.WriteLine($"Initial: {initial}, Medial: {medial}, Final: {final}");

// Example 2: Using Open Korean Text to compose Korean character
var okt = new OktHangulUtil();

// Combine Korean jamo characters to form a complete syllable
Console.WriteLine($"{okt.Combine('\u3131', '\u3161', '\u3139')}");

// Interface defining common operations for Korean text processing
public interface IHangulUtil
{
    // Split a Korean syllable into its constituent jamo (consonant/vowel) parts
    void Split(char c, out char onset, out char vowel, out char coda);
    
    // Combine individual jamo characters into a complete Korean syllable
    char Combine(char onset, char vowel, char coda);
}

// Implementation using KKMA (Korean Knowledge Morpheme Analyzer) library
public sealed class KkmaHangulUtil : IHangulUtil
{
    public void Split(char c, out char onset, out char vowel, out char coda)
    {
        // Use KKMA's split method to decompose Korean character
        var result = KkmaHangul.split(c);
        onset = result.cho;   // Initial consonant (초성)
        vowel = result.jung;  // Medial vowel (중성)
        coda = result.jong;   // Final consonant (종성)
    }

    public char Combine(char onset, char vowel, char coda)
        // Use KKMA's combine method to create Korean syllable from jamo parts
        => KkmaHangul.combine(onset, vowel, coda);
}

// Implementation using Open Korean Text processor library
public sealed class OktHangulUtil : IHangulUtil
{
    public void Split(char c, out char onset, out char vowel, out char coda)
    {
        // Use OKT's decomposeHangul method to break down Korean character
        var result = OktHangul.decomposeHangul(c);
        onset = result.onset();  // Initial consonant
        vowel = result.vowel();  // Medial vowel
        coda = result.coda();    // Final consonant
    }

    public char Combine(char onset, char vowel, char coda)
        // Use OKT's composeHangul method to build Korean syllable from components
        => OktHangul.composeHangul(onset, vowel, coda);
}
