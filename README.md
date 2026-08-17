# cross-platform-microphone

Turn your iPhone into a wireless microphone for your Windows PC.

## About this project

This project lets you connect your iPhone's microphone to your Windows desktop, streaming audio over your local network without the use of bluetooth or cables.

Existing tools for turning a phone into a PC microphone (WO Mic, KDE Connect, etc.) work, but tend to have rough setup, noticeable audio delay, or user reported concerns about suspicious background processes. This project aims to solve the core "phone mic over network" problem cleanly, then build outward from there. Our project is free and ad-free as well, unlike some alternatives.

## Status

As of now, this project is in early development and is not yet functional.

Currently designing project structure and communication protocol between the desktop and mobile apps.

## Tech stack

- **Desktop app (Windows):** C# / .NET
- **Mobile app (iOS):** Swift
- **Communication:** custom network protocol (in progress... see `/docs`)

## Roadmap

- [x] Repo setup
- [x] Define phone ↔ PC communication protocol
- [x] Desktop app: receive and play audio stream
- [x] Desktop app: capture and send audio stream (mock sender, standing in for real phone)
- [x] Desktop app: expose received audio as a virtual microphone device (via VB-CABLE)
- [ ] Desktop app: user interface (MicUI)
- [ ] Mobile app: capture and send audio stream (blocked. requires Mac access)
- [ ] Future: webcam, keyboard/mouse control, clipboard sync, file transfer, ...

## Contributors

- Owen Yang
- Aiden Wang

