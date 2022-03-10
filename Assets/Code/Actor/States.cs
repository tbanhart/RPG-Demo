public enum State{
    // Fallthrough case for if something goes wrong
    DEFAULT, 
    // No interaction or desination assigned
    IDLE, 
    // Going to a destination with no interaction specified
    MOVING, 
    // Processing an attack
    ATTACKING,
    // Going to an interaction destination
    TRAVELLING,
    // For executing an interaction
    INTERACTING,
    // Picking up an item
    GRAB,
    // Viewing an items inventory
    INVENTORY
    }