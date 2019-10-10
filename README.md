![Travis-CI](https://travis-ci.org/sbradl/CleanArchitecture.Requestor.svg?branch=master)

# CleanArchitecture.Requestor
C# implementation of the Clean Architecture Requestor component.

# Why?
The requestor component exists to decouple the controllers from the implementations of use cases and their concrete request data structures. 
The benefit of this approach is that the controllers do not always need to be recompiled whenever the implementation of a use case changes.



# What's in there?
The component contains 4 parts.

![uml](http://www.plantuml.com/plantuml/svg/bL712i8m3BttAq9F2leBWeg21o-27n2xE8gwrQGJHVRlrguJD_LW3YbftajU4dD44PWwfvH3Sy2SG6_eN97uaZNS5GIpXIVUeo5OfLp_UoiqHw64Vzgbj1aihcGgJuEM1joymmYheu_EH1a9DQliCIFk5zjGRYLq78XdM-HGu8b6iAmA0xk6CNmHNWrZhHHqYMjquBXqnDwI_52KvbgY2Uku_Q5etp3U5BKhcZEXfnIzCpyazDQXuvIYOJoUT0_HKwbDRD4YKrRLIeKxW-Qa-faE-MwNhv1kVVK0)

## Request Interface
This is a degenerate (empty) interface. It's implementations contain the actual data required by the corresponding use case.

## UseCase Interface
This interface contains the *Execute* method which will be called by the controller. It receives the request data structure as its input.
Implementations of this interface have to downcast the request object to the correct type. According to the clean architecture type-safety is sacrificed at this point to allow independent deployability.

## RequestBuilder
This class functions as a registry for different request builders. These builders are supposed to be registered by "main" (the actual application component which wire everything together).

## UseCaseFactory
Similar to the request builder this class functions as a registry for different factory functions which create use case instances.
