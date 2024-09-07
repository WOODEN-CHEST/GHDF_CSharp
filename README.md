# GHDF_CSharp
 CSharp implementation of the GHDF container format, featuring an encoder and decoder.
 
The GHDF Specification can be found here: https://docs.google.com/document/d/1e2523j0pj-Fv1vwdmtOLBKGt58k50Z5arSL22luoWbU/edit?usp=sharing

There is a key differences between the specification and this implementation:
The maximum length of arrays, compound and strings is 2^31 - 1 rather than 2^64 - 1.
