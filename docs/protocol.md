# Protocol Spec

## Format
**Raw PCM**(no compression)
- Prioritizes low delay over data size. Appropriate for this project which uses local Wi-Fi with bandwidth to spare.
- No compression needed since there is a surplus of bandwidth. Will only serve to add delay.

## Sample Rate
**48kHz** 
- Standard communication convention.
- Could be made adjustable for different purposes in the future.

## Chunking
**~20 ms per packet**
- The industry standard

## Delivery
**UDP**
- UDP rather than TCP with sequence numbers for loss detection. Live audio should skip lost moments rather than delay or potentially freeze.
- Could be made changeable for different purposes in the future.

## Port
**5500**
- Arbitrary number. Stays outside the reserved range of 1024
- Flexible. Can be changed later on.