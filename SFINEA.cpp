#include <string>
#include <iostream>

#define ASSIGN(Type, id)																				\
	template<typename = Type>																			\
	inline static void id(...) {}																		\
																										\
	template<typename T##Type = Type>																	\
	inline static void id(T##Type *t, decltype(T##Type::id) val) {										\
		t->id = val;																					\
	}																									\
																										\
	template<typename T##Type = Type, typename F##Type, typename Arg##Type>								\
	inline static void id(T##Type *t, F##Type f, Arg##Type arg, decltype(T##Type::id)* = nullptr) {		\
		f(arg, #id, reinterpret_cast<void**>(&t->id));													\
	}

template<typename Type>
class Assign {
public:
	ASSIGN(Type, f);
	ASSIGN(Type, g);
	ASSIGN(Type, h);
};

void f() {
	std::cout << "Call   : f" << std::endl;
}

void g() {
	std::cout << "Call   : g" << std::endl;
}

void h() {
	std::cout << "Call   : h" << std::endl;
}

void assign(int i, const char *name, void **id) {
	std::cout << "Assign : " << name << std::endl;
	switch (i) {
	case 0:
		*id = ::f;
		break;
	case 1:
		*id = ::g;
		break;
	case 2:
		*id = ::h;
		break;
	}
}

template<typename T>
void setFunc(T *t) {
	Assign<T>::f<>(t, ::f);
	Assign<T>::g<>(t, assign, 1);
	Assign<T>::h<>(t, assign, 2);
}

struct FuncF {
	void(*f)();
};

struct FuncG {
	void(*g)();
};

struct FuncH {
	void(*h)();
};

struct FuncFG {
	void(*f)();
	void(*g)();
};

struct FuncFH {
	void(*f)();
	void(*h)();
};

struct FuncGH {
	void(*g)();
	void(*h)();
};

struct FuncFGH {
	void(*f)();
	void(*g)();
	void(*h)();
};

int main() {
	FuncF funcF{};
	FuncG funcG{};
	FuncH funcH{};
	FuncFG funcFG{};
	FuncFH funcFH{};
	FuncGH funcGH{};
	FuncFGH funcFGH{};

	setFunc(&funcF);
	setFunc(&funcG);
	setFunc(&funcH);
	setFunc(&funcFG);
	setFunc(&funcFH);
	setFunc(&funcGH);
	setFunc(&funcFGH);

	funcF.f();
	funcG.g();
	funcH.h();
	funcFG.f();
	funcFG.g();
	funcFH.f();
	funcFH.h();
	funcGH.g();
	funcGH.h();
	funcFGH.f();
	funcFGH.g();
	funcFGH.h();
	return 0;
}