.PHONY: test

all: compile

compile:
	./build-mono

clean:
	rm -rf IronSmarkets/bin IronSmarkets/obj
	rm -rf IronSmarkets.Tests/bin IronSmarkets.Tests/obj
	rm -rf IronSmarkets.ConsoleExample/bin IronSmarkets.ConsoleExample/obj

distclean: clean
	git clean -f -x -d

test: all
	./run-tests-mono
