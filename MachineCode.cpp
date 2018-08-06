#include <Windows.h>
#include <iostream>
#include <memory>

// Add Function x86 Machine Code
unsigned char fcode[] = {
	0x55, 0x8B, 0xEC, 0x8B, 0x45, 0x08, 0x03, 0x45, 0x0C, 0x5D, 0xC3
};

int(*f)(int, int);

template<typename T, typename... Ts>
void print(T t, Ts... ts) {
	std::cout << t;
	print(ts...);
}

template<typename T = char*>
void print(T t = "") {
	std::cout << t << std::endl;
}

template<typename F, typename FC, UINT N>
DWORD setFuncDef(F &f, FC (&fc)[N]) {
	DWORD op;
	VirtualProtect(&f, N, PAGE_EXECUTE_READWRITE, &op);
	f = (F)&fc;
	return op;
}

int main() {
	DWORD oldProtect = setFuncDef(f, fcode);
	print(f(5, 6));
	return 0;
}