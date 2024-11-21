# Changelog

## [2.0.0-rc.2](https://github.com/rivet-gg/plugin-unity/compare/v2.0.0-rc.1...v2.0.0-rc.2) (2024-11-21)


### Features

* enable using plugin without auth ([#55](https://github.com/rivet-gg/plugin-unity/issues/55)) ([04d8d7a](https://github.com/rivet-gg/plugin-unity/commit/04d8d7af125361a7ba519cb9a3013cd801407027))


### Bug Fixes

* add meta for macos plugin ([#75](https://github.com/rivet-gg/plugin-unity/issues/75)) ([3b71ab9](https://github.com/rivet-gg/plugin-unity/commit/3b71ab91787fc33c3fb377cfed756aa2677c7056))
* docker deploy ([#68](https://github.com/rivet-gg/plugin-unity/issues/68)) ([701befe](https://github.com/rivet-gg/plugin-unity/commit/701befeddacfc565ee9d424b659361bd6c5d1f15))
* don't focus logs windows unintentionally ([#61](https://github.com/rivet-gg/plugin-unity/issues/61)) ([23e1820](https://github.com/rivet-gg/plugin-unity/commit/23e1820d6c635037ccea246f60f8d8ed7b1a2a16))
* expose backend endpoint and game version to local game server via env ([#59](https://github.com/rivet-gg/plugin-unity/issues/59)) ([b1a73b9](https://github.com/rivet-gg/plugin-unity/commit/b1a73b9fabfa2f3f424e47a134270c94a539468a))
* fix build_dev.ts & run_dev.ts on windows ([#70](https://github.com/rivet-gg/plugin-unity/issues/70)) ([14d351d](https://github.com/rivet-gg/plugin-unity/commit/14d351da74ef1b807d505e251fbfac8b7eeda648))
* fix path for client executable on windows ([#73](https://github.com/rivet-gg/plugin-unity/issues/73)) ([54219d1](https://github.com/rivet-gg/plugin-unity/commit/54219d18d3efafab49722c19e0264701b9cffd2a))
* ignore rivet dylibs ([#71](https://github.com/rivet-gg/plugin-unity/issues/71)) ([362facb](https://github.com/rivet-gg/plugin-unity/commit/362facbfc7064e515c29c5bbab75197d93e3198f))
* pass correct build slug to game server ([#64](https://github.com/rivet-gg/plugin-unity/issues/64)) ([3630cac](https://github.com/rivet-gg/plugin-unity/commit/3630cac465de77acaebc4bd6156e85fd10fc69cd))
* update bootstrap type ([#41](https://github.com/rivet-gg/plugin-unity/issues/41)) ([0814650](https://github.com/rivet-gg/plugin-unity/commit/08146509f069507e8e25796c9e501f807949cee6))
* update steps dropdown name ([#69](https://github.com/rivet-gg/plugin-unity/issues/69)) ([a43473b](https://github.com/rivet-gg/plugin-unity/commit/a43473beb355366191a7dbfd4eff5c75cbf2daf4))


### Chores

* add cli binaries ([#47](https://github.com/rivet-gg/plugin-unity/issues/47)) ([24bd9c9](https://github.com/rivet-gg/plugin-unity/commit/24bd9c936869fda8fc39be91a8ab0672df46eecb))
* add current build to env list ([#66](https://github.com/rivet-gg/plugin-unity/issues/66)) ([08eaf84](https://github.com/rivet-gg/plugin-unity/commit/08eaf84409fd050db9217af466fab9b7e734695b))
* add native toolchain libs ([#40](https://github.com/rivet-gg/plugin-unity/issues/40)) ([a2d80b0](https://github.com/rivet-gg/plugin-unity/commit/a2d80b07704a2fda3a06a997bfb477456fb441f9))
* add release script ([#72](https://github.com/rivet-gg/plugin-unity/issues/72)) ([c6bd5c5](https://github.com/rivet-gg/plugin-unity/commit/c6bd5c5a1e3b671962376032c40ad2f95718af58))
* add spawning clients ([#44](https://github.com/rivet-gg/plugin-unity/issues/44)) ([3fb0fb7](https://github.com/rivet-gg/plugin-unity/commit/3fb0fb7d7fc683d63ba50a485aa06a03901ca3cc))
* allow hooking existing game server process ([#62](https://github.com/rivet-gg/plugin-unity/issues/62)) ([d98702f](https://github.com/rivet-gg/plugin-unity/commit/d98702f7afd2d7ce11a42abaca2fc1d6febe1806))
* disable modules tab ([#63](https://github.com/rivet-gg/plugin-unity/issues/63)) ([ef6623c](https://github.com/rivet-gg/plugin-unity/commit/ef6623c74f363345cc2f6a3d70f8bccc8bef57cc))
* fix dpeloy ([#67](https://github.com/rivet-gg/plugin-unity/issues/67)) ([c3ef972](https://github.com/rivet-gg/plugin-unity/commit/c3ef9725aaf3adfba4826a54c89e52ec3af8d8c4))
* get deploys working ([#48](https://github.com/rivet-gg/plugin-unity/issues/48)) ([a712701](https://github.com/rivet-gg/plugin-unity/commit/a712701ec977d19a2520204d32debd73de1a2c7e))
* handle alerts better ([#49](https://github.com/rivet-gg/plugin-unity/issues/49)) ([ce0704a](https://github.com/rivet-gg/plugin-unity/commit/ce0704a5f6c12680cee0e7fab20a5da7ba0a5bab))
* impl play type selector ([#57](https://github.com/rivet-gg/plugin-unity/issues/57)) ([28329b2](https://github.com/rivet-gg/plugin-unity/commit/28329b23be039133ffbfffabe40b0d9cb34c2175))
* implement polling toolchain task events instead of pushing ([#60](https://github.com/rivet-gg/plugin-unity/issues/60)) ([5b9b75d](https://github.com/rivet-gg/plugin-unity/commit/5b9b75d2cd9be334dc30e780f6f7829d967e4017))
* make task outputs more legible ([#58](https://github.com/rivet-gg/plugin-unity/issues/58)) ([8e53346](https://github.com/rivet-gg/plugin-unity/commit/8e533460d5880123e1675e225dec9ec2e351a3fe))
* re-impl deploys ([#45](https://github.com/rivet-gg/plugin-unity/issues/45)) ([b3f4a81](https://github.com/rivet-gg/plugin-unity/commit/b3f4a814e3e323e25de8e36a8cf128ae25620565))
* refactor global code in to RivetGlobal ([#54](https://github.com/rivet-gg/plugin-unity/issues/54)) ([49dc983](https://github.com/rivet-gg/plugin-unity/commit/49dc9838ce0f15a52ce6d35d958b5b7110e7852e))
* release 2.0.0-rc.2 ([b222c4f](https://github.com/rivet-gg/plugin-unity/commit/b222c4fe25a6d1ea6cdf3dbfb413d4390247a0a2))
* remove committed dylibs ([#56](https://github.com/rivet-gg/plugin-unity/issues/56)) ([d0f1f53](https://github.com/rivet-gg/plugin-unity/commit/d0f1f5353499bb7e61836f43fc7b70aadff58955))
* remove deploy tab ([#52](https://github.com/rivet-gg/plugin-unity/issues/52)) ([16c5764](https://github.com/rivet-gg/plugin-unity/commit/16c5764129560091678e43188f15ea4ff0a1adc5))
* restore backend & game server log panels ([#53](https://github.com/rivet-gg/plugin-unity/issues/53)) ([3c2b0fd](https://github.com/rivet-gg/plugin-unity/commit/3c2b0fdf99812b5e823462e8a05e3e32cee7c8d1))
* rewrite config export ([#42](https://github.com/rivet-gg/plugin-unity/issues/42)) ([0415de2](https://github.com/rivet-gg/plugin-unity/commit/0415de247b00435e37e37b92b040d204c3fd6724))
* switch back to show term for now ([#43](https://github.com/rivet-gg/plugin-unity/issues/43)) ([882ab04](https://github.com/rivet-gg/plugin-unity/commit/882ab046a914c13c112f19a943c3c04c82cae018))
* tweak deploy ui ([#46](https://github.com/rivet-gg/plugin-unity/issues/46)) ([9feab8a](https://github.com/rivet-gg/plugin-unity/commit/9feab8aed566aeada25779cb8b13d0af5de8ed60))
* update deploy + remote env buttons to match authed state ([#65](https://github.com/rivet-gg/plugin-unity/issues/65)) ([846734a](https://github.com/rivet-gg/plugin-unity/commit/846734af7d67b9f7fe92a2535b0dca9904afad44))
* update develop & deploy & settings & modules ui to match new spec ([#51](https://github.com/rivet-gg/plugin-unity/issues/51)) ([1b6092d](https://github.com/rivet-gg/plugin-unity/commit/1b6092db1899fa2c2619ffa6ef8cd442ba84c2d2))
* update ffi task interface ([#50](https://github.com/rivet-gg/plugin-unity/issues/50)) ([79c8fa7](https://github.com/rivet-gg/plugin-unity/commit/79c8fa7584aac59cf25ecb8de5ce4d061d018709))

## [2.0.0-rc.1](https://github.com/rivet-gg/plugin-unity/compare/v1.0.3...v2.0.0-rc.1) (2024-08-13)


### Chores

* add game server & backend panel ([#37](https://github.com/rivet-gg/plugin-unity/issues/37)) ([865b967](https://github.com/rivet-gg/plugin-unity/commit/865b96778b7af63edf6a47227ae3c0eda548e72a))
* add popup ([#36](https://github.com/rivet-gg/plugin-unity/issues/36)) ([77e6324](https://github.com/rivet-gg/plugin-unity/commit/77e6324b0647b878ee528102e555de652ae4e8a0))
* bump CLI version ([#31](https://github.com/rivet-gg/plugin-unity/issues/31)) ([57745df](https://github.com/rivet-gg/plugin-unity/commit/57745dfcc265858c81e6b40af63d057709d54438))
* migrate to ui toolkit ([#34](https://github.com/rivet-gg/plugin-unity/issues/34)) ([a362b69](https://github.com/rivet-gg/plugin-unity/commit/a362b694dc89db1574295b4c967c6a1f5d17de5a))
* refactor on to toolchain ([#33](https://github.com/rivet-gg/plugin-unity/issues/33)) ([31df965](https://github.com/rivet-gg/plugin-unity/commit/31df9652b9aa41f85508a0474c0e2c62b938d0b4))
* release 2.0.0-rc.1 ([890fce4](https://github.com/rivet-gg/plugin-unity/commit/890fce44b16b18b5e71b471ba47cf8ee3eecffde))
* switch to toolchain ([#35](https://github.com/rivet-gg/plugin-unity/issues/35)) ([7e5da4d](https://github.com/rivet-gg/plugin-unity/commit/7e5da4dd2bdb51f7330cc22e827d1b9c32a22524))

## [1.0.3](https://github.com/rivet-gg/plugin-unity/compare/v1.0.2...v1.0.3) (2024-03-14)


### Bug Fixes

* Bump unity package version with release ([2a1eb18](https://github.com/rivet-gg/plugin-unity/commit/2a1eb18522d39ae4e764d8a92a9ba931b1a06d04))


### Continuous Integration

* Fix release please finding config file ([6e6a2fc](https://github.com/rivet-gg/plugin-unity/commit/6e6a2fc8dd726e51159f5bf8aaebba878701d8c5))


### Chores

* Fix release please manifest file name ([c071769](https://github.com/rivet-gg/plugin-unity/commit/c0717699cb1131475278061564bebaceabfa1284))
* Fix version string ([8e0bc96](https://github.com/rivet-gg/plugin-unity/commit/8e0bc96e87caec8315207b677ca01f206a73bde0))
* Update package author ([1c863d8](https://github.com/rivet-gg/plugin-unity/commit/1c863d8a062d798d74f91b01f75f2893da8833d9))

## [1.0.2](https://github.com/rivet-gg/plugin-unity/compare/v1.0.1...v1.0.2) (2024-03-13)


### Bug Fixes

* Release please action checks out repo on release for build ([610ab76](https://github.com/rivet-gg/plugin-unity/commit/610ab765da5fff9b2d0183a2ebe380719e5bc915))

## [1.0.1](https://github.com/rivet-gg/plugin-unity/compare/v1.0.0...v1.0.1) (2024-03-13)


### Bug Fixes

* Fix mention of Godot in readme ([af6a53c](https://github.com/rivet-gg/plugin-unity/commit/af6a53c4397551b18edc3d492022831250468f2b))

## [1.0.0](https://github.com/rivet-gg/plugin-unity/compare/v1.0.0-rc2...v1.0.0) (2024-03-07)


### Miscellaneous Chores

* release 1.0.0 ([98668eb](https://github.com/rivet-gg/plugin-unity/commit/98668eb884863643afbe3a36c077cebb779c2650))
* release 1.0.0-rc.3 ([#15](https://github.com/rivet-gg/plugin-unity/issues/15)) ([0c34633](https://github.com/rivet-gg/plugin-unity/commit/0c34633c3e617cde113143e042510eabfe29528c))

## [1.0.0-rc2](https://github.com/rivet-gg/plugin-unity/compare/v1.0.0-rc1...v1.0.0-rc2) (2024-03-06)


### Bug Fixes

* Build with correct Rivet config file ([#10](https://github.com/rivet-gg/plugin-unity/issues/10)) ([aeb2898](https://github.com/rivet-gg/plugin-unity/commit/aeb28986dcaf7141b20f5829b57bbb4d96e7a987))
* Properly include RivetToken and ApiEndpoint with game build ([#7](https://github.com/rivet-gg/plugin-unity/issues/7)) ([f6307e7](https://github.com/rivet-gg/plugin-unity/commit/f6307e72426fbe7a3a535f5f0e345371ee6da81a))


### Miscellaneous Chores

* release 1.0.0-rc2 ([#11](https://github.com/rivet-gg/plugin-unity/issues/11)) ([e8f8d74](https://github.com/rivet-gg/plugin-unity/commit/e8f8d74ca2c1934b3210aa0777b41e5ba5b2a619))

## 1.0.0-rc1 (2024-03-02)


### Features

* Cleanup readme for initial release ([#6](https://github.com/rivet-gg/plugin-unity/issues/6)) ([de63d64](https://github.com/rivet-gg/plugin-unity/commit/de63d6441b7bf5a3c4d200fd34d967c402980aaf))
* Set up release please ([#3](https://github.com/rivet-gg/plugin-unity/issues/3)) ([fda20f0](https://github.com/rivet-gg/plugin-unity/commit/fda20f0fc41b47c245c87cd271bac7493b8cf075))
* Unity plugin foundations ([0b0482e](https://github.com/rivet-gg/plugin-unity/commit/0b0482e9cac07ea79afce5f21d924b19f79d7781))


### Miscellaneous Chores

* release 1.0.0-rc1 ([#4](https://github.com/rivet-gg/plugin-unity/issues/4)) ([832f1ee](https://github.com/rivet-gg/plugin-unity/commit/832f1ee27e307aa0d6900ef978e3310e5948f286))
